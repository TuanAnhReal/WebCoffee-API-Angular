namespace WebCoffee.ViewModels.Catalog.KhuVucBans
{
    public class BanVm
    {
        public string SoBan { get; set; }
        public string SoKV { get; set; }
        public string TenBan { get; set; }
        public string TrangThaiBan { get; set; }
        public string? GhiChuBAN { get; set; }

        public TimeSpan? ThoiGianDatSắpToi { get; set; }
        public string TenKhachDat { get; set; }
    }
}