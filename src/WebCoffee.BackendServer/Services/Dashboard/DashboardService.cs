using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;
using WebCoffee.ViewModels.Catalog.Dashboard;

namespace WebCoffee.BackendServer.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        // ════════════════════════════════════════════════════════════════════════
        //  SUMMARY – Gộp KPI + Chart + Lists trong 1 request/response
        //  QUAN TRỌNG: Không dùng Task.WhenAll — EF Core DbContext không thread-safe.
        //  Ba method chạy tuần tự trên cùng 1 DbContext instance.
        // ════════════════════════════════════════════════════════════════════════
        public async Task<DashboardSummaryVm> GetSummaryAsync()
        {
            var kpi   = await GetKpiAsync();
            var chart = await GetChartAsync();
            var lists = await GetListsAsync();

            return new DashboardSummaryVm
            {
                Kpi          = kpi,
                RevenueChart = chart,
                Lists        = lists
            };
        }

        // ════════════════════════════════════════════════════════════════════════
        //  KPI
        // ════════════════════════════════════════════════════════════════════════
        public async Task<DashboardKpiVm> GetKpiAsync()
        {
            var now           = DateTime.Now;
            var todayStart    = DateTime.Today;
            var todayEnd      = todayStart.AddDays(1);
            var yesterdayStart= todayStart.AddDays(-1);
            var fifteenMinsAgo= now.AddMinutes(-15);
            var twoHoursAgo   = now.AddHours(-2);
            var next24h       = now.AddHours(24);

            // ── 1. HÓA ĐƠN + CHI TIẾT: 2 ngày trong 1 LEFT JOIN ────────────────
            var rawOrders = await (
                from hd in _context.HoaDons.AsNoTracking()
                join ct in _context.CTHDs.AsNoTracking() on hd.SoHD equals ct.SoHD into ctGroup
                from ct in ctGroup.DefaultIfEmpty()
                where hd.TGVao >= yesterdayStart && hd.TGVao < todayEnd
                select new
                {
                    hd.SoHD,
                    hd.TrangThaiHD,
                    TongTien  = hd.TongTien  ?? 0m,
                    TGVao     = hd.TGVao,
                    Qty       = ct != null ? (ct.SLSP      ?? 0)  : 0,
                    GiaVon    = ct != null ?  ct.GiaVon           : 0m,
                    GiamGia   = ct != null ? (ct.GiamGia   ?? 0m) : 0m,
                    ThanhTien = ct != null ? (ct.ThanhTien ?? 0m) : 0m
                }
            ).ToListAsync();

            // ── 2. PHÂN TÁCH in-memory ──────────────────────────────────────────
            var todayPaid = rawOrders
                .Where(x => x.TGVao >= todayStart && x.TrangThaiHD == "Đã thanh toán")
                .ToList();

            var todayAll = rawOrders
                .Where(x => x.TGVao >= todayStart)
                .GroupBy(x => x.SoHD)
                .ToList();

            var yesterdayPaid = rawOrders
                .Where(x => x.TGVao < todayStart && x.TrangThaiHD == "Đã thanh toán")
                .ToList();

            var yesterdayAll = rawOrders
                .Where(x => x.TGVao < todayStart)
                .GroupBy(x => x.SoHD)
                .ToList();

            var revenueToday     = todayPaid.GroupBy(x => x.SoHD).Sum(g => g.First().TongTien);
            var revenueYesterday = yesterdayPaid.GroupBy(x => x.SoHD).Sum(g => g.First().TongTien);

            var costToday     = (decimal)todayPaid.Sum(x => x.Qty * (double)x.GiaVon);
            var costYesterday = (decimal)yesterdayPaid.Sum(x => x.Qty * (double)x.GiaVon);

            var profitToday     = revenueToday     - costToday;
            var profitYesterday = revenueYesterday - costYesterday;

            var invoiceToday     = todayAll.Count;
            var invoiceYesterday = yesterdayAll.Count;

            var pendingOrders = rawOrders
                .Where(x => x.TGVao >= todayStart
                         && x.TrangThaiHD == "Chờ pha chế"
                         && x.TGVao < fifteenMinsAgo)
                .Select(x => x.SoHD)
                .Distinct()
                .Count();

            var discountAmountToday   = todayPaid.Sum(x => x.GiamGia);
            var promotionRevenueToday = todayPaid.Where(x => x.GiamGia > 0).Sum(x => x.ThanhTien);

            // ── 3. KHÁCH HÀNG ────────────────────────────────────────────────────
            var customerDates = await _context.KhachHangs.AsNoTracking()
                .Where(x => x.NgayTao >= yesterdayStart && x.NgayTao < todayEnd)
                .Select(x => (DateTime?)x.NgayTao)
                .ToListAsync();

            var customerToday     = customerDates.Count(x => x >= todayStart);
            var customerYesterday = customerDates.Count(x => x < todayStart);

            // ── 4. BÀN ───────────────────────────────────────────────────────────
            var tables = await _context.Bans.AsNoTracking()
                .Select(x => new { x.TenBan, x.TrangThaiBan })
                .ToListAsync();

            var totalTables  = tables.Count;
            var activeTables = tables.Count(x => x.TrangThaiBan != "Trống");

            var longServingTables = await _context.HoaDons.AsNoTracking()
                .Where(x => x.TrangThaiHD == "Chưa thanh toán"
                         && x.SoBan != null
                         && x.TGVao < twoHoursAgo)
                .Select(x => x.SoBan)
                .Distinct()
                .CountAsync();

            // ── 5. KHUYẾN MÃI – tuần tự, không WhenAll trên cùng DbContext ───────
            var activePromotions = await _context.KhuyenMais.AsNoTracking()
                .CountAsync(x => x.NgayBD <= now && x.NgayKT >= now);

            var promotionProducts = await _context.SanPham_KhuyenMais.AsNoTracking()
                .CountAsync();

            var expiringPromos = await _context.KhuyenMais.AsNoTracking()
                .CountAsync(x => x.NgayKT >= now && x.NgayKT <= next24h);

            // ── 6. GROWTH RATES ─────────────────────────────────────────────────
            static double CalcGrowth(decimal today, decimal yesterday) =>
                yesterday == 0m ? 0 : Math.Round((double)((today - yesterday) * 100m / yesterday), 1);

            static double CalcGrowthInt(int today, int yesterday) =>
                yesterday == 0 ? 0 : Math.Round((today - yesterday) * 100.0 / yesterday, 1);

            // ── 7. CẢNH BÁO VẬN HÀNH ────────────────────────────────────────────
            var alerts = new List<DashboardAlertVm>();

            if (pendingOrders > 0)
                alerts.Add(new DashboardAlertVm
                {
                    Type     = "PENDING_ORDER",
                    Message  = $"Có {pendingOrders} hóa đơn chờ pha chế quá 15 phút",
                    Count    = pendingOrders,
                    Severity = AlertSeverity.Danger
                });

            if (longServingTables > 0)
                alerts.Add(new DashboardAlertVm
                {
                    Type     = "LONG_SERVING",
                    Message  = $"Có {longServingTables} bàn phục vụ quá 2 giờ",
                    Count    = longServingTables,
                    Severity = AlertSeverity.Warning
                });

            if (expiringPromos > 0)
                alerts.Add(new DashboardAlertVm
                {
                    Type     = "EXPIRING_PROMO",
                    Message  = $"Có {expiringPromos} khuyến mãi sắp hết hạn trong 24h",
                    Count    = expiringPromos,
                    Severity = AlertSeverity.Warning
                });

            return new DashboardKpiVm
            {
                SnapshotAt = now,

                RevenueToday     = revenueToday,
                RevenueYesterday = revenueYesterday,
                RevenueGrowth    = CalcGrowth(revenueToday, revenueYesterday),

                CostToday       = costToday,
                ProfitToday     = profitToday,
                ProfitYesterday = profitYesterday,
                ProfitGrowth    = CalcGrowth(profitToday, profitYesterday),

                InvoiceToday     = invoiceToday,
                InvoiceYesterday = invoiceYesterday,
                InvoiceGrowth    = CalcGrowthInt(invoiceToday, invoiceYesterday),
                PendingOrders    = pendingOrders,

                CustomerToday     = customerToday,
                CustomerYesterday = customerYesterday,
                CustomerGrowth    = CalcGrowthInt(customerToday, customerYesterday),

                TotalTables       = totalTables,
                ActiveTables      = activeTables,
                LongServingTables = longServingTables,

                ActivePromotions      = activePromotions,
                PromotionProducts     = promotionProducts,
                ExpiringPromos        = expiringPromos,
                DiscountAmountToday   = discountAmountToday,
                PromotionRevenueToday = promotionRevenueToday,

                Alerts = alerts
            };
        }

        // ════════════════════════════════════════════════════════════════════════
        //  LISTS
        // ════════════════════════════════════════════════════════════════════════
        public async Task<DashboardListsVm> GetListsAsync()
        {
            // ── BÀN ──────────────────────────────────────────────────────────────
            var allTables = await _context.Bans.AsNoTracking()
                .Select(x => new { x.TenBan, x.TrangThaiBan })
                .ToListAsync();

            var totalTables  = allTables.Count;
            var activeTables = allTables.Count(x => x.TrangThaiBan != "Trống");
            var tableStatuses = allTables
                .Where(x => x.TrangThaiBan != "Trống")
                .Take(10)
                .Select(x => new TableStatusVm
                {
                    TableName   = x.TenBan       ?? "",
                    Status      = x.TrangThaiBan ?? "",
                    ServingTime = "--"
                })
                .ToList();

            // ── TOP PRODUCTS ────────────────────────────────────────────────────
            var topProducts = await (
                from ct in _context.CTHDs.AsNoTracking()
                join sp in _context.SanPhams.AsNoTracking() on ct.MaSP equals sp.MaSP
                group new { ct, sp } by sp.TenSP into g
                orderby g.Sum(x => x.ct.SLSP) descending
                select new TopProductVm
                {
                    ProductName = g.Key ?? "",
                    Quantity    = g.Sum(x => x.ct.SLSP)      ?? 0,
                    Revenue     = g.Sum(x => x.ct.ThanhTien) ?? 0m
                }
            ).Take(5).ToListAsync();

            // ── TOP PROMOTIONS ──────────────────────────────────────────────────
            var topPromotions = await (
                from km   in _context.KhuyenMais.AsNoTracking()
                join spkm in _context.SanPham_KhuyenMais.AsNoTracking() on km.MaKM   equals spkm.MaKM
                join ct   in _context.CTHDs.AsNoTracking()               on spkm.MaSP equals ct.MaSP
                join hd   in _context.HoaDons.AsNoTracking()             on ct.SoHD   equals hd.SoHD
                where hd.TrangThaiHD == "Đã thanh toán"
                group new { ct, hd } by new { km.MaKM, km.TenKM } into g
                select new TopPromotionVm
                {
                    PromoName   = g.Key.TenKM ?? "",
                    OrdersCount = g.Select(x => x.hd.SoHD).Distinct().Count(),
                    Revenue     = g.Sum(x => (decimal?)x.ct.ThanhTien) ?? 0m
                }
            ).OrderByDescending(x => x.Revenue).Take(5).ToListAsync();

            // ── RECENT INVOICES ─────────────────────────────────────────────────
            var recentInvoices = await _context.HoaDons.AsNoTracking()
                .OrderByDescending(x => x.TGVao)
                .Take(5)
                .Select(x => new RecentInvoiceVm
                {
                    InvoiceId = x.SoHD        ?? "",
                    Amount    = x.TongTien    ?? 0m,
                    Status    = x.TrangThaiHD ?? ""
                })
                .ToListAsync();

            return new DashboardListsVm
            {
                TotalTables    = totalTables,
                ActiveTables   = activeTables,
                TableStatuses  = tableStatuses,
                TopProducts    = topProducts,
                TopPromotions  = topPromotions,
                RecentInvoices = recentInvoices
            };
        }

        // ════════════════════════════════════════════════════════════════════════
        //  CHART – Doanh thu 7 ngày gần nhất
        // ════════════════════════════════════════════════════════════════════════
        public async Task<List<decimal>> GetChartAsync()
        {
            var today     = DateTime.Today;
            var startDate = today.AddDays(-6);
            var endDate   = today.AddDays(1);

            var dailyRevenues = await _context.HoaDons.AsNoTracking()
                .Where(x => x.TrangThaiHD == "Đã thanh toán"
                         && x.TGVao >= startDate
                         && x.TGVao < endDate)
                .GroupBy(x => x.TGVao!.Value.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(x => (decimal?)x.TongTien) ?? 0m })
                .ToListAsync();

            return Enumerable.Range(0, 7)
                .Select(i => today.AddDays(i - 6))
                .Select(day => dailyRevenues.FirstOrDefault(x => x.Date == day)?.Total ?? 0m)
                .ToList();
        }
    }
}
