using Microsoft.AspNetCore.Http;

namespace WebCoffee.ViewModels.Catalog.SanPhams
{
    public class SanPhamCreateRequest
    {
        public string TenSp { get; set; }
        public string MaLoaiSp { get; set; }
        public decimal GiaVon { get; set; }
        public decimal DonGia { get; set; }
        public string Dvt { get; set; }
        public string KichThuoc { get; set; }
        public string? MoTa { get; set; }
        public string TrangThai { get; set; }
        public IFormFile? HinhAnhFile { get; set; }
    }
}