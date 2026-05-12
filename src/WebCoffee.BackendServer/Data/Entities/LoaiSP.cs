using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("LOAISP")]
public class LoaiSP
{
    [Key][MaxLength(10)] public string MaLoaiSP { get; set; } = null!;
    [Required][MaxLength(255)] public string TenLoaiSP { get; set; } = null!;
    public bool? LaHangDeVo { get; set; }
    [MaxLength(255)] public string? BaoQuan { get; set; }
    [MaxLength(255)] public string? GhiChuLoaiSP { get; set; }

    public ICollection<SanPham>? SanPhams { get; set; }
}