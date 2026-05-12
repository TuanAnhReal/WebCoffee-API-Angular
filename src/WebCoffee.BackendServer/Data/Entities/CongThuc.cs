using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("CONGTHUC")]
public class CongThuc
{
    [Key][MaxLength(10)] public string MaCT { get; set; } = null!;
    [MaxLength(10)] public string? MaSP { get; set; }
    [MaxLength(10)] public string? MaNL { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? SoLuongNL { get; set; }
    [MaxLength(255)] public string? GhiChuCT { get; set; }

    [ForeignKey("MaSP")] public SanPham? SanPham { get; set; }
    [ForeignKey("MaNL")] public NguyenLieu? NguyenLieu { get; set; }
}