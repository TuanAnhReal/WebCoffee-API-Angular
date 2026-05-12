using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("THANHTOAN")]
public class ThanhToan
{
    [Key][MaxLength(10)] public string MaTT { get; set; } = null!;
    [MaxLength(10)] public string? SoHD { get; set; }
    [MaxLength(50)] public string? PhuongThucTT { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? SoTienTT { get; set; }
    public DateTime? ThoiGianTT { get; set; }
    [MaxLength(50)] public string? TrangThaiTT { get; set; }

    [ForeignKey("SoHD")] public HoaDon? HoaDon { get; set; }
}