using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("KHUYENMAI")]
public class KhuyenMai
{
    [Key][MaxLength(10)] public string MaKM { get; set; } = null!;
    [MaxLength(255)] public string? TenKM { get; set; }
    [MaxLength(50)] public string? LoaiKM { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? GiaTriKM { get; set; }
    public DateTime? NgayBD { get; set; }
    public DateTime? NgayKT { get; set; }
    [MaxLength(255)] public string? DieuKienKM { get; set; }
    [MaxLength(50)] public string? TrangThaiKM { get; set; }
    [MaxLength(255)] public string? GhiChuKM { get; set; }

    public ICollection<SanPham_KhuyenMai>? SanPham_KhuyenMais { get; set; }
}