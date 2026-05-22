using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities;

[Table("REFRESHTOKEN")]
public class RefreshToken
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Token { get; set; } = null!;
    [Required]
    [MaxLength(50)]
    public string TenDangNhap { get; set; } = null!;
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedAt { get; set; }
    [ForeignKey("TenDangNhap")]
    public virtual TaiKhoan? TaiKhoan { get; set; }
}