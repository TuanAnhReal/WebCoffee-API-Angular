using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("CTPHIEUNHAP_NL")]
public class CTPhieuNhapNL
{
    [Key][MaxLength(10)] public string SoCTPNNL { get; set; } = null!;
    [MaxLength(10)] public string? SoPNNL { get; set; }
    [MaxLength(10)] public string? MaNL { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? SLNhapNL { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? DonGiaNhapNL { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? ThanhTienNhapNL { get; set; }

    [ForeignKey("SoPNNL")] public PhieuNhapNL? PhieuNhapNL { get; set; }
    [ForeignKey("MaNL")] public NguyenLieu? NguyenLieu { get; set; }
}