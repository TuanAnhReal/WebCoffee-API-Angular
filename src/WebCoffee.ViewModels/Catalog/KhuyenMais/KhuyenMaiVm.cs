using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCoffee.ViewModels.Catalog.KhuyenMais
{
    public class KhuyenMaiVm
    {
        public string MaKM { get; set; }
        public string TenKM { get; set; }
        public string LoaiKM { get; set; }
        public decimal? GiaTriKM { get; set; }
        public DateTime? NgayBD { get; set; }
        public DateTime? NgayKT { get; set; }
        public string? DieuKienKM { get; set; }
        public string? TrangThaiKM { get; set; }
        public string? GhiChuKM { get; set; }
    }
}
