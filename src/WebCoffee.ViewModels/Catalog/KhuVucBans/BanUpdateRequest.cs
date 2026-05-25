using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCoffee.ViewModels.Catalog.KhuVucBans
{
    public class BanUpdateRequest
    {
        public string SoKV { get; set; }
        public string TenBan { get; set; }
        public string TrangThaiBan { get; set; }
        public string? GhiChuBAN { get; set; }
    }
}
