using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("NHANVIEN")]
public class NhanVien
{
    [Key][MaxLength(10)] public string MaNV { get; set; } = null!;
    [Required][MaxLength(10)] public string MaLoaiNV { get; set; } = null!;
    [Required][MaxLength(30)] public string HoNV { get; set; } = null!;
    [Required][MaxLength(20)] public string TenNV { get; set; } = null!;
    public bool? PhaiNV { get; set; }
    public DateTime? NgaySinhNV { get; set; }
    [MaxLength(15)] public string? SoDTNV { get; set; }
    [MaxLength(255)] public string? DiaChiNV_TT { get; set; }
    [MaxLength(255)] public string? DiaChiNV_NT { get; set; }
    [MaxLength(20)] public string? SoCCCD { get; set; }
    [MaxLength(30)] public string? SoTKNV { get; set; }
    [MaxLength(50)] public string? TenNgHNV { get; set; }
    [MaxLength(30)] public string? SoBHYT { get; set; }
    [MaxLength(30)] public string? SoBHXH { get; set; }
    [MaxLength(255)] public string? HinhAnhNV { get; set; }
    [MaxLength(50)] public string? TrinhDoHV { get; set; }
    [MaxLength(255)] public string? GhiChuNV { get; set; }
    [MaxLength(50)] public string? TrangThaiNV { get; set; }

    [ForeignKey("MaLoaiNV")] public LoaiNV? LoaiNV { get; set; }
    public TaiKhoan? TaiKhoan { get; set; }
}