using System;
using System.Collections.Generic;

namespace WebCoffee.ViewModels.Catalog.KhuVucBans
{
    public class KhuVucVm
    {
        public string SoKV { get; set; }
        public string TenKV { get; set; }
        public TimeSpan? TGMo { get; set; }
        public TimeSpan? TGDong { get; set; }
        public string TrangThaiKhu { get; set; }
        public decimal? PhuThuKV { get; set; }
        public string? GhiChuKV { get; set; }

        public List<BanVm> Bans { get; set; } = new List<BanVm>();
    }
}