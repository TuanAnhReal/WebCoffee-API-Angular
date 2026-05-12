using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("SANPHAM")]
public class SanPham
{
    [Key][MaxLength(10)] public string MaSP { get; set; } = null!;
    [Required][MaxLength(10)] public string MaLoaiSP { get; set; } = null!;
    [Required][MaxLength(255)] public string TenSP { get; set; } = null!;
    [Required][Column(TypeName = "decimal(18,2)")] public decimal DonGia { get; set; }
    [MaxLength(20)] public string? DVT { get; set; }
    [MaxLength(50)] public string? KichThuoc { get; set; }
    [MaxLength(255)] public string? HinhAnh { get; set; }
    [MaxLength(255)] public string? MoTa { get; set; }
    [MaxLength(50)] public string? TrangThaiSP { get; set; }

    [ForeignKey("MaLoaiSP")] public LoaiSP? LoaiSP { get; set; }
    public ICollection<SanPham_KhuyenMai>? SanPham_KhuyenMais { get; set; }
    public ICollection<CongThuc>? CongThucs { get; set; }
}