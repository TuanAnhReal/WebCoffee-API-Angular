using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("TAIKHOAN")]
public class TaiKhoan
{
    [Key][MaxLength(50)] public string TenDangNhap { get; set; } = null!;
    [Required][MaxLength(10)] public string MaNV { get; set; } = null!;
    [Required][MaxLength(10)] public string MaPQ { get; set; } = null!;
    [Required][MaxLength(255)] public string MatKhau { get; set; } = null!;
    [MaxLength(50)] public string? TrangThaiTK { get; set; }
    public DateTime? LanDangNhapCuoi { get; set; }

    [ForeignKey("MaNV")] public NhanVien? NhanVien { get; set; }
    [ForeignKey("MaPQ")] public PhanQuyen? PhanQuyen { get; set; }
}