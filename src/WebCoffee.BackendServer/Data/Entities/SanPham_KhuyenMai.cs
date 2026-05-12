using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("SANPHAM_KHUYENMAI")]
public class SanPham_KhuyenMai
{
    [MaxLength(10)] public string MaSP { get; set; } = null!;
    [MaxLength(10)] public string MaKM { get; set; } = null!;

    [ForeignKey("MaSP")] public SanPham? SanPham { get; set; }
    [ForeignKey("MaKM")] public KhuyenMai? KhuyenMai { get; set; }
}