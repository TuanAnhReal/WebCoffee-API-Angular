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

        public async Task<DashboardKpiVm> GetKpiAsync()
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);

            #region KPI CHÍNH

            var revenueToday = await _context.HoaDons
                .AsNoTracking()
                .Where(x =>
                    x.TrangThaiHD == "Đã thanh toán" &&
                    x.TGVao.HasValue &&
                    x.TGVao.Value.Date == today)
                .SumAsync(x => (decimal?)x.TongTien) ?? 0;

            var revenueYesterday = await _context.HoaDons
                .AsNoTracking()
                .Where(x =>
                    x.TrangThaiHD == "Đã thanh toán" &&
                    x.TGVao.HasValue &&
                    x.TGVao.Value.Date == yesterday)
                .SumAsync(x => (decimal?)x.TongTien) ?? 0;

            var invoiceToday = await _context.HoaDons
                .AsNoTracking()
                .CountAsync(x =>
                    x.TGVao.HasValue &&
                    x.TGVao.Value.Date == today);

            var invoiceYesterday = await _context.HoaDons
                .AsNoTracking()
                .CountAsync(x =>
                    x.TGVao.HasValue &&
                    x.TGVao.Value.Date == yesterday);

            var customerToday = await _context.KhachHangs
                .AsNoTracking()
                .CountAsync(x => x.NgayTao.Date == today);

            var customerYesterday = await _context.KhachHangs
                .AsNoTracking()
                .CountAsync(x => x.NgayTao.Date == yesterday);

            var costToday = await (
                from hd in _context.HoaDons.AsNoTracking()
                join ct in _context.CTHDs.AsNoTracking()
                    on hd.SoHD equals ct.SoHD
                where hd.TrangThaiHD == "Đã thanh toán"
                      && hd.TGVao.HasValue
                      && hd.TGVao.Value.Date == today
                select (decimal?)(ct.SLSP * ct.GiaVon)
            ).SumAsync() ?? 0;

            var costYesterday = await (
                from hd in _context.HoaDons.AsNoTracking()
                join ct in _context.CTHDs.AsNoTracking()
                    on hd.SoHD equals ct.SoHD
                where hd.TrangThaiHD == "Đã thanh toán"
                      && hd.TGVao.HasValue
                      && hd.TGVao.Value.Date == yesterday
                select (decimal?)(ct.SLSP * ct.GiaVon)
            ).SumAsync() ?? 0;

            var profitToday = revenueToday - costToday;
            var profitYesterday = revenueYesterday - costYesterday;

            double revenueGrowth =
                revenueYesterday == 0
                    ? 0
                    : (double)((revenueToday - revenueYesterday) * 100 / revenueYesterday);

            double invoiceGrowth =
                invoiceYesterday == 0
                    ? 0
                    : (double)((invoiceToday - invoiceYesterday) * 100 / invoiceYesterday);

            double customerGrowth =
                customerYesterday == 0
                    ? 0
                    : (double)((customerToday - customerYesterday) * 100 / customerYesterday);

            double profitGrowth =
                profitYesterday == 0
                    ? 0
                    : (double)((profitToday - profitYesterday) * 100 / profitYesterday);

            #endregion

            #region ALERTS
            var alerts = new List<string>();

            // Hóa đơn chờ pha chế > 15 phút
            var pendingOrders = await _context.HoaDons.AsNoTracking()
                .CountAsync(x => x.TrangThaiHD == "Chờ pha chế" && x.TGVao < DateTime.Now.AddMinutes(-15));
            if (pendingOrders > 0) alerts.Add($"Có {pendingOrders} hóa đơn chờ pha chế > 15 phút");

            // Bàn đang phục vụ > 2 giờ
            // Lưu ý: Nếu DB của bạn lưu TGBatDauPhucVu thì dùng nó, ở đây tôi tạm dùng TGVao của hóa đơn chưa thanh toán gần nhất của bàn
            var longServingTables = await (
                from hd in _context.HoaDons.AsNoTracking()
                where hd.TrangThaiHD == "Chưa thanh toán" && hd.SoBan != null && hd.TGVao < DateTime.Now.AddHours(-2)
                select hd.SoBan
            ).Distinct().CountAsync();
            if (longServingTables > 0) alerts.Add($"Có {longServingTables} bàn phục vụ > 2 giờ");

            // KM sắp hết hạn trong 24h
            var expiringPromos = await _context.KhuyenMais.AsNoTracking()
                .CountAsync(x => x.NgayKT <= DateTime.Now.AddHours(24) && x.NgayKT >= DateTime.Now);
            if (expiringPromos > 0) alerts.Add($"Có {expiringPromos} khuyến mãi sắp hết hạn trong 24h");
            #endregion

            #region KHUYẾN MÃI

            var activePromotions = await _context.KhuyenMais.AsNoTracking()
                .CountAsync(x => x.NgayBD <= DateTime.Now && x.NgayKT >= DateTime.Now);

            var promotionProducts = await _context.SanPham_KhuyenMais.AsNoTracking().CountAsync();

            var discountAmountToday = await (
                from hd in _context.HoaDons.AsNoTracking()
                join ct in _context.CTHDs.AsNoTracking() on hd.SoHD equals ct.SoHD
                where hd.TrangThaiHD == "Đã thanh toán" && hd.TGVao.HasValue && hd.TGVao.Value.Date == today
                select (decimal?)ct.GiamGia
            ).SumAsync() ?? 0;

            var promotionRevenueToday = await (
                from hd in _context.HoaDons.AsNoTracking()
                join ct in _context.CTHDs.AsNoTracking() on hd.SoHD equals ct.SoHD
                join spkm in _context.SanPham_KhuyenMais.AsNoTracking() on ct.MaSP equals spkm.MaSP
                join km in _context.KhuyenMais.AsNoTracking() on spkm.MaKM equals km.MaKM
                where hd.TrangThaiHD == "Đã thanh toán"
                      && hd.TGVao.HasValue && hd.TGVao.Value.Date == today
                      && km.NgayBD <= DateTime.Now && km.NgayKT >= DateTime.Now
                select (decimal?)ct.ThanhTien
            ).SumAsync() ?? 0;
            #endregion

            return new DashboardKpiVm
            {
                RevenueToday = revenueToday,
                RevenueGrowth = Math.Round(revenueGrowth, 1),

                InvoiceToday = invoiceToday,
                InvoiceGrowth = Math.Round(invoiceGrowth, 1),

                CustomerToday = customerToday,
                CustomerGrowth = Math.Round(customerGrowth, 1),

                ProfitToday = profitToday,
                ProfitGrowth = Math.Round(profitGrowth, 1),
                ActivePromotions = activePromotions,
                PromotionProducts = promotionProducts,
                DiscountAmountToday = discountAmountToday,
                PromotionRevenueToday = promotionRevenueToday,
            };

        }

        public async Task<DashboardListsVm> GetListsAsync()
        {

            #region BÀN

            var totalTables = await _context.Bans
                .AsNoTracking()
                .CountAsync();

            var activeTables = await _context.Bans
                .AsNoTracking()
                .CountAsync(x => x.TrangThaiBan != "Trống");

            var tableStatuses = await _context.Bans
                .AsNoTracking()
                .Where(x => x.TrangThaiBan != "Trống")
                .Take(10)
                .Select(x => new TableStatusVm
                {
                    TableName = x.TenBan,
                    Status = x.TrangThaiBan ?? "",
                    ServingTime = "--"
                })
                .ToListAsync();

            #endregion

            #region TOP PRODUCTS

            var topProducts = await (
                from ct in _context.CTHDs
                join sp in _context.SanPhams
                    on ct.MaSP equals sp.MaSP
                group new { ct, sp } by sp.TenSP into g
                orderby g.Sum(x => x.ct.SLSP) descending
                select new TopProductVm
                {
                    ProductName = g.Key,
                    Quantity = g.Sum(x => x.ct.SLSP) ?? 0,
                    Revenue = g.Sum(x => x.ct.ThanhTien) ?? 0
                }
            )
            .Take(5)
            .ToListAsync();

            #endregion

            #region TOP PROMOTIONS

            var topPromotions = await (
                from km in _context.KhuyenMais.AsNoTracking()
                join spkm in _context.SanPham_KhuyenMais.AsNoTracking() on km.MaKM equals spkm.MaKM
                join ct in _context.CTHDs.AsNoTracking() on spkm.MaSP equals ct.MaSP
                join hd in _context.HoaDons.AsNoTracking() on ct.SoHD equals hd.SoHD
                where hd.TrangThaiHD == "Đã thanh toán"
                group new { ct, hd } by new { km.MaKM, km.TenKM } into g
                select new TopPromotionVm
                {
                    PromoName = g.Key.TenKM,
                    OrdersCount = g.Select(x => x.hd.SoHD).Distinct().Count(),
                    Revenue = g.Sum(x => (decimal?)x.ct.ThanhTien) ?? 0
                }
            )
            .OrderByDescending(x => x.Revenue)
            .Take(5)
            .ToListAsync();
            #endregion

            #region RECENT INVOICES

            var recentInvoices = await (
                from hd in _context.HoaDons.AsNoTracking()
                orderby hd.TGVao descending
                select new RecentInvoiceVm
                {
                    InvoiceId = hd.SoHD,
                    Amount = hd.TongTien ?? 0,
                    Status = hd.TrangThaiHD ?? ""
                }
            )
            .Take(5)
            .ToListAsync();

            #endregion

            return new DashboardListsVm
            {
                ActiveTables = activeTables,
                TotalTables = totalTables,

                TopProducts = topProducts,
                TopPromotions = topPromotions,
                RecentInvoices = recentInvoices,
                TableStatuses = tableStatuses
            };
        }
        public async Task<List<decimal>> GetChartAsync()
        {
            var today = DateTime.Today;
            var startDate = today.AddDays(-6);
            var endDate = today.AddDays(1);

            var dailyRevenues = await _context.HoaDons.AsNoTracking()
                .Where(x => x.TrangThaiHD == "Đã thanh toán" && x.TGVao >= startDate && x.TGVao < endDate)
                .GroupBy(x => x.TGVao.Value.Date)
                .Select(g => new {
                    Date = g.Key,
                    Total = g.Sum(x => (decimal?)x.TongTien) ?? 0
                })
                .ToListAsync();

            var revenueChart = new List<decimal>();
            for (int i = 6; i >= 0; i--)
            {
                var day = today.AddDays(-i);
                var rev = dailyRevenues.FirstOrDefault(x => x.Date == day)?.Total ?? 0;
                revenueChart.Add(rev);
            }

            return revenueChart;
        }
    }
}