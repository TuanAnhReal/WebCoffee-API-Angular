using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCoffee.ViewModels.Catalog
{
    // 1. Model để trả dữ liệu về (Get)
    public class SanPhamVm
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public decimal DonGia { get; set; }
        public string HinhAnh { get; set; }
        public string? TrangThaiSP { get; set; }
    }

    // 2. Model để hứng dữ liệu thêm mới (Create)
    public class SanPhamCreateRequest
    {
        public string? TenSP { get; set; }
        public decimal DonGia { get; set; }
        public string? MaLoaiSP { get; set; }
    }

    // 3. Model để hứng dữ liệu cập nhật (Update)
    public class SanPhamUpdateRequest
    {
        public string? TenSP { get; set; }
        public decimal DonGia { get; set; }
        public string? TrangThaiSP { get; set; }
    }
}
