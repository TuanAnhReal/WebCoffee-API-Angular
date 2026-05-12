using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("BAN")]
public class Ban
{
    [Key][MaxLength(10)] public string SoBan { get; set; } = null!;
    [MaxLength(10)] public string? SoKV { get; set; }
    [MaxLength(50)] public string? TenBan { get; set; }
    [MaxLength(50)] public string? TrangThaiBan { get; set; }
    [MaxLength(255)] public string? GhiChuBAN { get; set; }

    [ForeignKey("SoKV")] public KhuVuc? KhuVuc { get; set; }
}