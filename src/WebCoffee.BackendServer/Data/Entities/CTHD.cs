using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("CTHD")]
public class CTHD
{
    // ===== Thông tin hiện có =====
    [Key][MaxLength(10)] public string SoCTHD { get; set; } = null!;
    [MaxLength(10)] public string? SoHD { get; set; }
    [MaxLength(10)] public string? MaSP { get; set; }

    public int? SLSP { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal? DonGia { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? GiamGia { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? ThanhTien { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal GiaVon { get; set; }

    // ===== Snapshot sản phẩm =====
    [Column(TypeName = "decimal(18,2)")]
    public decimal? GiaGoc { get; set; }

    // ===== Snapshot khuyến mãi =====
    [MaxLength(10)]
    public string? MaKM { get; set; }

    [Column(TypeName = "nvarchar(255)")]
    public string? TenKM { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string? LoaiKM { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? GiaTriKM { get; set; }

    // ===== Navigation =====
    [ForeignKey("SoHD")] public HoaDon? HoaDon { get; set; }
    [ForeignKey("MaSP")] public SanPham? SanPham { get; set; }
}