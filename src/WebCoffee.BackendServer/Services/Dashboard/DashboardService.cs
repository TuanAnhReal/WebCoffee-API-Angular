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

        public async Task<DashboardVm> GetDashboardSummaryAsync()
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

            #region KHUYẾN MÃI

            var activePromotions = await _context.KhuyenMais
                .AsNoTracking()
                .CountAsync(x =>
                    x.NgayBD <= DateTime.Now &&
                    x.NgayKT >= DateTime.Now);

            var promotionProducts = await _context.SanPham_KhuyenMais
                .AsNoTracking()
                .CountAsync();

            decimal discountAmountToday = 0;
            decimal promotionRevenueToday = 0;

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

            var topPromotions = await _context.KhuyenMais
                .AsNoTracking()
                .Select(x => new TopPromotionVm
                {
                    PromoName = x.TenKM,
                    OrdersCount = 0,
                    Revenue = 0
                })
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

            #region REVENUE CHART

            var revenueChart = new List<decimal>();

            for (int i = 6; i >= 0; i--)
            {
                var day = today.AddDays(-i);

                var revenue = await _context.HoaDons
                    .AsNoTracking()
                    .Where(x =>
                        x.TrangThaiHD == "Đã thanh toán" &&
                        x.TGVao.HasValue &&
                        x.TGVao.Value.Date == day)
                    .SumAsync(x => (decimal?)x.TongTien) ?? 0;

                revenueChart.Add(revenue);
            }

            #endregion

            return new DashboardVm
            {
                RevenueToday = revenueToday,
                RevenueGrowth = Math.Round(revenueGrowth, 1),

                InvoiceToday = invoiceToday,
                InvoiceGrowth = Math.Round(invoiceGrowth, 1),

                CustomerToday = customerToday,
                CustomerGrowth = Math.Round(customerGrowth, 1),

                ProfitToday = profitToday,
                ProfitGrowth = Math.Round(profitGrowth, 1),

                ActiveTables = activeTables,
                TotalTables = totalTables,

                ActivePromotions = activePromotions,
                PromotionProducts = promotionProducts,
                DiscountAmountToday = discountAmountToday,
                PromotionRevenueToday = promotionRevenueToday,

                TopProducts = topProducts,
                TopPromotions = topPromotions,
                TableStatuses = tableStatuses,
                RecentInvoices = recentInvoices,

                RevenueChart = revenueChart
            };
        }
    }
}