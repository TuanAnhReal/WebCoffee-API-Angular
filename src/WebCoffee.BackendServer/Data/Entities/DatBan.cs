using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("DATBAN")]
public class DatBan
{
    [Key][MaxLength(10)] public string MaDatBan { get; set; } = null!;
    [MaxLength(10)] public string? MaKH { get; set; }
    [MaxLength(10)] public string? SoBan { get; set; }
    public DateTime? NgayDat { get; set; }
    public TimeSpan? GioDat { get; set; }
    public int? SoLuongNguoi { get; set; }
    [MaxLength(50)] public string? TrangThaiDat { get; set; }
    [MaxLength(255)] public string? GhiChuDat { get; set; }

    [ForeignKey("MaKH")] public KhachHang? KhachHang { get; set; }
    [ForeignKey("SoBan")] public Ban? Ban { get; set; }
}