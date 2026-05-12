using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("NCC")]
public class NhaCungCap
{
    [Key][MaxLength(10)] public string MaNCC { get; set; } = null!;
    [Required][MaxLength(255)] public string TenNCC { get; set; } = null!;
    [MaxLength(255)] public string? DiaChiNCC { get; set; }
    [MaxLength(30)] public string? SoTKNCC { get; set; }
    [MaxLength(15)] public string? SDTNCC { get; set; }
    [MaxLength(255)] public string? GhiChuNCC { get; set; }
}