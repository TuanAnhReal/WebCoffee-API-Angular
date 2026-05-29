namespace WebCoffee.ViewModels.Catalog.HoaDons
{
    public class HoaDonCreateRequest
    {
        public string MaKH { get; set; }
        public string SoBan { get; set; }
        public string MaNV_PV { get; set; }
        public string? MaNV_PC { get; set; }
        public decimal GiamGiaHD { get; set; }
        public decimal PhuThu { get; set; }
        public decimal ThueVAT { get; set; }
        public string TrangThaiHD { get; set; }
        public string GhiChuHD { get; set; }

        public List<CTHDCreateRequest> ChiTietHoaDons { get; set; } = new List<CTHDCreateRequest>();
    }
}