namespace WebCoffee.ViewModels.Catalog.Dashboard
{
    public class DashboardVm
    {
        public decimal DoanhThuHomNay { get; set; }
        public double PhanTramDoanhThu { get; set; }

        public int TongĐonHangHomNay { get; set; }
        public double PhanTramĐonHang { get; set; }
        public int KhachHangMoiHomNay { get; set; }
        public double PhanTramKhachHang { get; set; }

        public decimal LoiNhuanHomNay { get; set; }
        public double PhanTramLoiNhuan { get; set; }

        public List<GiaoDichGanDayVm> GiaoDichGanDay { get; set; } = new List<GiaoDichGanDayVm>();
    }

    public class GiaoDichGanDayVm
    {
        public string SoHD { get; set; }
        public string TenKhachHang { get; set; }
        public DateTime ThoiGian { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
    }
}