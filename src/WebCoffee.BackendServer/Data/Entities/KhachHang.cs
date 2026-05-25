using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("KH")]
public class KhachHang
{
    [Key][MaxLength(10)] public string MaKH { get; set; } = null!;
    [MaxLength(100)] public string? TenKH { get; set; }
    [MaxLength(15)] public string? SDTKH { get; set; }
    public int? DiemTichLuy { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;
    [MaxLength(255)] public string? GhiChuKH { get; set; }
}