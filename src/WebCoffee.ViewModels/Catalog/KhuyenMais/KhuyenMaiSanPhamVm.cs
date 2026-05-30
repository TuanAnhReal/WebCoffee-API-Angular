namespace WebCoffee.ViewModels.Catalog.KhuyenMais
{
    public class KhuyenMaiSanPhamVm
    {
        public string MaSP { get; set; } = string.Empty;

        public string TenSP { get; set; } = string.Empty;

        public decimal DonGia { get; set; }

        public string? LoaiKM { get; set; }

        public decimal? GiaTriKM { get; set; }

        public decimal GiaSauKhuyenMai { get; set; }
    }
}