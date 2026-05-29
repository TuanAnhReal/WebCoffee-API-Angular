using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities
{
    [Table("CHAMCONG")]
    public class ChamCong
    {
        [Key]
        [StringLength(20)]
        [Column("MaChamCong")]
        public string MaChamCong { get; set; }

        [Required]
        [StringLength(10)]
        [Column("MaNV")]
        public string MaNV { get; set; }

        [Required]
        [StringLength(10)]
        [Column("MaCaLam")]
        public string MaCaLam { get; set; }

        [Required]
        [Column("TgChamCong")]
        public DateTime TgChamCong { get; set; }

        // Khóa ngoại liên kết tới bảng Nhân Viên (Giả định bảng của bạn tên là NhanVien)
        [ForeignKey("MaNV")]
        public virtual NhanVien NhanVien { get; set; }

        // Khóa ngoại liên kết tới bảng Ca Làm
        [ForeignKey("MaCaLam")]
        public virtual CaLam CaLam { get; set; }
    }
}