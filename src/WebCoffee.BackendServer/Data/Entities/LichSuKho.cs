using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("LICHSU_KHO")]
public class LichSuKho
{
    [Key][MaxLength(10)] public string MaLSKho { get; set; } = null!;
    [MaxLength(10)] public string? MaNL { get; set; }
    [MaxLength(10)] public string? NguoiThucHien { get; set; }
    [MaxLength(50)] public string? LoaiGD { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? SoLuongGD { get; set; }
    public DateTime? ThoiGianGD { get; set; }
    [MaxLength(255)] public string? GhiChuLSK { get; set; }

    [ForeignKey("MaNL")] public NguyenLieu? NguyenLieu { get; set; }
    [ForeignKey("NguoiThucHien")] public NhanVien? NhanVien { get; set; }
}