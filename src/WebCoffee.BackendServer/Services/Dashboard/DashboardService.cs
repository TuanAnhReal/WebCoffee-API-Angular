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
            var today = DateTime.Now.Date;
            var yesterday = today.AddDays(-1);

            var doanhThuHomNay = await _context.HoaDons
                .Where(x => x.TrangThaiHD == "Đã thanh toán" && x.TGVao.HasValue && x.TGVao.Value.Date == today)
                .SumAsync(x => (decimal?)x.TongTien) ?? 0;

            var doanhThuHomQua = await _context.HoaDons
                .Where(x => x.TrangThaiHD == "Đã thanh toán" && x.TGVao.HasValue && x.TGVao.Value.Date == yesterday)
                .SumAsync(x => (decimal?)x.TongTien) ?? 0;

            var donHomNay = await _context.HoaDons
                .Where(x => x.TGVao.HasValue && x.TGVao.Value.Date == today)
                .CountAsync();

            var donHomQua = await _context.HoaDons
                .Where(x => x.TGVao.HasValue && x.TGVao.Value.Date == yesterday)
                .CountAsync();

            var khachMoiHomNay = await _context.KhachHangs
                .Where(x => x.NgayTao.Date == today)
                .CountAsync();

            var khachMoiHomQua = await _context.KhachHangs
                .Where(x => x.NgayTao.Date == yesterday)
                .CountAsync();

            var giaVonHomNay = await (from hd in _context.HoaDons
                                      join ct in _context.CTHDs on hd.SoHD equals ct.SoHD
                                      where hd.TrangThaiHD == "Đã thanh toán" && hd.TGVao.HasValue && hd.TGVao.Value.Date == today
                                      select (decimal?)(ct.SLSP * ct.GiaVon)).SumAsync() ?? 0;

            var giaVonHomQua = await (from hd in _context.HoaDons
                                      join ct in _context.CTHDs on hd.SoHD equals ct.SoHD
                                      where hd.TrangThaiHD == "Đã thanh toán" && hd.TGVao.HasValue && hd.TGVao.Value.Date == yesterday
                                      select (decimal?)(ct.SLSP * ct.GiaVon)).SumAsync() ?? 0;

            var loiNhuanHomNay = doanhThuHomNay - giaVonHomNay;
            var loiNhuanHomQua = doanhThuHomQua - giaVonHomQua;

            var giaoDichGanDay = await (from hd in _context.HoaDons
                                        join kh in _context.KhachHangs on hd.MaKH equals kh.MaKH into khGroup
                                        from kh in khGroup.DefaultIfEmpty()
                                        orderby hd.TGVao descending
                                        select new GiaoDichGanDayVm
                                        {
                                            SoHD = hd.SoHD,
                                            TenKhachHang = kh != null ? kh.TenKH : "Khách lẻ",
                                            ThoiGian = hd.TGVao ?? DateTime.Now,
                                            TongTien = hd.TongTien ?? 0,
                                            TrangThai = hd.TrangThaiHD
                                        }).Take(5).ToListAsync();

            double phanTramDoanhThu = doanhThuHomQua == 0 ? 0 : (double)((doanhThuHomNay - doanhThuHomQua) * 100 / doanhThuHomQua);
            double phanTramDon = donHomQua == 0 ? 0 : (double)((donHomNay - donHomQua) * 100 / donHomQua);
            double phanTramKhach = khachMoiHomQua == 0 ? 0 : (double)((khachMoiHomNay - khachMoiHomQua) * 100 / khachMoiHomQua);
            double phanTramLoiNhuan = loiNhuanHomQua == 0 ? 0 : (double)((loiNhuanHomNay - loiNhuanHomQua) * 100 / loiNhuanHomQua);

            return new DashboardVm
            {
                DoanhThuHomNay = doanhThuHomNay,
                PhanTramDoanhThu = Math.Round(phanTramDoanhThu, 1),
                TongĐonHangHomNay = donHomNay,
                PhanTramĐonHang = Math.Round(phanTramDon, 1),
                KhachHangMoiHomNay = khachMoiHomNay,
                PhanTramKhachHang = Math.Round(phanTramKhach, 1),
                LoiNhuanHomNay = loiNhuanHomNay,
                PhanTramLoiNhuan = Math.Round(phanTramLoiNhuan, 1),
                GiaoDichGanDay = giaoDichGanDay
            };
        }
    }
}