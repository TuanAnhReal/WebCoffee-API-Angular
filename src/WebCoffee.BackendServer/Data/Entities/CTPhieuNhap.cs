using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("CTPNHAP")]
public class CTPhieuNhap
{
    [Key][MaxLength(10)] public string SoCTPN { get; set; } = null!;
    [MaxLength(10)] public string? SoPN { get; set; }
    [MaxLength(10)] public string? MaSP { get; set; }
    public int? SLN { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? DonGiaNhap { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? ThanhTienNhap { get; set; }
    [MaxLength(255)] public string? GhiChuCTPN { get; set; }

    [ForeignKey("SoPN")] public PhieuNhap? PhieuNhap { get; set; }
    [ForeignKey("MaSP")] public SanPham? SanPham { get; set; }
}