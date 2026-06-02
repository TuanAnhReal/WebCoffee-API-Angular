namespace WebCoffee.ViewModels.Catalog.HoaDons
{
    public class HoaDonVm
    {
        public string SoHD { get; set; }
        public string? MaKH { get; set; }
        public string? SoBan { get; set; }
        public string? MaNV_PV { get; set; }
        public string? MaNV_PC { get; set; }
        public DateTime TGVao { get; set; }
        public DateTime? TGRa { get; set; }
        public decimal GiamGiaHD { get; set; }
        public decimal PhuThu { get; set; }
        public decimal ThueVAT { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThaiHD { get; set; }
        public string? GhiChuHD { get; set; }
        
        public List<CTHDVm> ChiTietHoaDons { get; set; } = new List<CTHDVm>();
    }
}