namespace WebCoffee.ViewModels.Catalog.KhuyenMais
{
    public class KhuyenMaiUpdateRequest
    {
        public string? TenKM { get; set; }
        public string? LoaiKM { get; set; }

        public decimal? GiaTriKM { get; set; }

        public DateTime? NgayBD { get; set; }

        public DateTime? NgayKT { get; set; }

        public string? DieuKienKM { get; set; }

        public string? GhiChuKM { get; set; }
    }
}