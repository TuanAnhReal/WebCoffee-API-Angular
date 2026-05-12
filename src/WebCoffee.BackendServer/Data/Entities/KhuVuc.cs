using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("KV")]
public class KhuVuc
{
    [Key][MaxLength(10)] public string SoKV { get; set; } = null!;
    [MaxLength(100)] public string? TenKV { get; set; }
    public TimeSpan? TGMo { get; set; }
    public TimeSpan? TGDong { get; set; }
    [MaxLength(50)] public string? TrangThaiKhu { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? PhuThuKV { get; set; }
    [MaxLength(255)] public string? GhiChuKV { get; set; }

    public ICollection<Ban>? Bans { get; set; }
}