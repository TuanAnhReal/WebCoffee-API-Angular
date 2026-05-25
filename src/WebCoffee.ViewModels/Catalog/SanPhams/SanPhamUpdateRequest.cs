using Microsoft.AspNetCore.Http;

namespace WebCoffee.ViewModels.Catalog.SanPhams
{
    public class SanPhamUpdateRequest
    {
        public string TenSp { get; set; }
        public decimal GiaSp { get; set; }
        public string MoTa { get; set; }
        public string TrangThai { get; set; }
        public string MaLoaiSp { get; set; }

        public IFormFile? HinhAnhFile { get; set; }
    }
}