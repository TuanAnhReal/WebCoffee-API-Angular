using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("CTHD")]
public class CTHD
{
    [Key][MaxLength(10)] public string SoCTHD { get; set; } = null!;
    [MaxLength(10)] public string? SoHD { get; set; }
    [MaxLength(10)] public string? MaSP { get; set; }
    public int? SLSP { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? DonGia { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? GiamGia { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? ThanhTien { get; set; }

    [ForeignKey("SoHD")] public HoaDon? HoaDon { get; set; }
    [ForeignKey("MaSP")] public SanPham? SanPham { get; set; }
}