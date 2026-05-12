using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("HOADON")]
public class HoaDon
{
    [Key][MaxLength(10)] public string SoHD { get; set; } = null!;
    [MaxLength(10)] public string? MaKH { get; set; }
    [MaxLength(10)] public string? SoBan { get; set; }
    [MaxLength(10)] public string? MaNV_PV { get; set; }
    [MaxLength(10)] public string? MaNV_PC { get; set; }
    public DateTime? TGVao { get; set; }
    public DateTime? TGRa { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? GiamGiaHD { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? PhuThu { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? ThueVAT { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? TongTien { get; set; }
    [MaxLength(50)] public string? TrangThaiHD { get; set; }
    [MaxLength(255)] public string? HoaDonKyNhan { get; set; }
    [MaxLength(255)] public string? GhiChuHD { get; set; }

    [ForeignKey("MaKH")] public KhachHang? KhachHang { get; set; }
    [ForeignKey("SoBan")] public Ban? Ban { get; set; }
    public NhanVien? NhanVienPhucVu { get; set; }
    public NhanVien? NhanVienPhaChe { get; set; }
    public ICollection<CTHD>? CTHDs { get; set; }
}