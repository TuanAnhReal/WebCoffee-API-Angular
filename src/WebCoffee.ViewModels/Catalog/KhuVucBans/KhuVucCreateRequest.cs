using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCoffee.ViewModels.Catalog.KhuVucBans
{
    public class KhuVucCreateRequest
    {
        public string SoKV { get; set; }
        public string TenKV { get; set; }
        public TimeSpan? TGMo { get; set; }
        public TimeSpan? TGDong { get; set; }
        public string TrangThaiKhu { get; set; }
        public decimal? PhuThuKV { get; set; }
        public string? GhiChuKV { get; set; }
    }
}
