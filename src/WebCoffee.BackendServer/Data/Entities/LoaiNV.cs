using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("LOAINV")]
public class LoaiNV
{
    [Key][MaxLength(10)] public string MaLoaiNV { get; set; } = null!;
    [Required][MaxLength(255)] public string TenLoaiNV { get; set; } = null!;
    public double? HsLoaiNV { get; set; }
    [MaxLength(255)] public string? CVLoaiNV { get; set; }
    [MaxLength(255)] public string? GhiChuLoaiNV { get; set; }

    public ICollection<NhanVien>? NhanViens { get; set; }
}