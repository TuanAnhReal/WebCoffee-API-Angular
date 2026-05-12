using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("PNHAP")]
public class PhieuNhap
{
    [Key][MaxLength(10)] public string SoPN { get; set; } = null!;
    [MaxLength(10)] public string? MaNCC { get; set; }
    [MaxLength(10)] public string? MaNV { get; set; }
    public DateTime? NgayNhap { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? TongTienNhap { get; set; }
    [MaxLength(255)] public string? NhapHang { get; set; }
    [MaxLength(255)] public string? GhiChuPN { get; set; }

    [ForeignKey("MaNCC")] public NhaCungCap? NhaCungCap { get; set; }
    [ForeignKey("MaNV")] public NhanVien? NhanVien { get; set; }
    public ICollection<CTPhieuNhap>? CTPhieuNhaps { get; set; }
}