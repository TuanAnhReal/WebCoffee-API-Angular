namespace WebCoffee.ViewModels.Catalog.SanPhams
{
    public class SanPhamVm
    {
        public string MaSp { get; set; }
        public string TenSp { get; set; }
        public decimal GiaVon { get; set; }
        public decimal GiaSp { get; set; }
        public string? Dvt { get; set; }
        public string? KichThuoc { get; set; }
        public string? MoTa { get; set; }
        public string TrangThai { get; set; }
        public string MaLoaiSp { get; set; }
        public string TenLoaiSp { get; set; }
        public string HinhAnh { get; set; }

        public bool CoKhuyenMai { get; set; }
        public string? MaKhuyenMai { get; set; }
        public string? TenKhuyenMai { get; set; }
        public string? LoaiKhuyenMai { get; set; }
        public decimal? GiaTriKhuyenMai { get; set; }
        public decimal GiaSauKhuyenMai { get; set; }
    }
}