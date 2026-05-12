using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("PHANQUYEN")]
public class PhanQuyen
{
    [Key][MaxLength(10)] public string MaPQ { get; set; } = null!;
    [Required][MaxLength(100)] public string TenPQ { get; set; } = null!;
    [MaxLength(255)] public string? MoTaPQ { get; set; }

    public ICollection<TaiKhoan>? TaiKhoans { get; set; }
}