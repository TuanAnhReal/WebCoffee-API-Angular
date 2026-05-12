using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("PHIEUNHAP_NL")]
public class PhieuNhapNL
{
    [Key][MaxLength(10)] public string SoPNNL { get; set; } = null!;
    [MaxLength(10)] public string? MaNCC { get; set; }
    [MaxLength(10)] public string? MaNV { get; set; }
    public DateTime? NgayNhapNL { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? TongTienNhapNL { get; set; }
    [MaxLength(255)] public string? GhiChuPNNL { get; set; }

    [ForeignKey("MaNCC")] public NhaCungCap? NhaCungCap { get; set; }
    [ForeignKey("MaNV")] public NhanVien? NhanVien { get; set; }
}