using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("NGUYENLIEU")]
public class NguyenLieu
{
    [Key][MaxLength(10)] public string MaNL { get; set; } = null!;
    [MaxLength(255)] public string? TenNL { get; set; }
    [MaxLength(20)] public string? DVTNL { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? SoLuongTon { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? MucCanhBao { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? DonGiaNL { get; set; }
    public DateTime? HanSuDung { get; set; }
    [MaxLength(50)] public string? TrangThaiNL { get; set; }
    [MaxLength(255)] public string? GhiChuNL { get; set; }

    public ICollection<CongThuc>? CongThucs { get; set; }
}