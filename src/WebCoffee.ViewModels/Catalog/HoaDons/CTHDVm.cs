namespace WebCoffee.ViewModels.Catalog.HoaDons
{
    public class CTHDVm
    {
        public string SoCTHD { get; set; }
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int SLSP { get; set; }
        public decimal GiaGoc { get; set; }
        public decimal DonGia { get; set; }
        public decimal GiamGia { get; set; }
        public decimal ThanhTien { get; set; }

        // Thông tin khuyến mãi
        public bool CoKhuyenMai { get; set; }
        public string? MaKhuyenMai { get; set; }
        public string? TenKhuyenMai { get; set; }
        public string? LoaiKhuyenMai { get; set; }
        public decimal? GiaTriKhuyenMai { get; set; }
    }
}