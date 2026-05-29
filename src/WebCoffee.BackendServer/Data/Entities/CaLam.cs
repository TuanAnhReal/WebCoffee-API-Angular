using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoffee.BackendServer.Data.Entities
{
    [Table("CALAM")]
    public class CaLam
    {
        [Key]
        [StringLength(10)]
        [Column("MaCaLam")]
        public string MaCaLam { get; set; }

        [Required]
        [StringLength(50)]
        [Column("TenCa")]
        public string TenCa { get; set; }

        [Required]
        [StringLength(8)] // Lưu dạng chuỗi "06:00:00" cho đơn giản và đồng bộ
        [Column("TgVaoCa")]
        public string TgVaoCa { get; set; }

        [Required]
        [StringLength(8)] // Lưu dạng chuỗi "14:00:00"
        [Column("TgRaCa")]
        public string TgRaCa { get; set; }

        // Mối quan hệ một ca làm có nhiều bản ghi chấm công
        public virtual ICollection<ChamCong> ChamCongs { get; set; }
    }
}