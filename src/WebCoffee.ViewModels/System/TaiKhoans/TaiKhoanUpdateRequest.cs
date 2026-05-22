namespace WebCoffee.ViewModels.System.TaiKhoans
{
    public class TaiKhoanUpdateRequest
    {
        public string MaNV { get; set; }
        public string MaPQ { get; set; }
        public string MatKhau { get; set; } // Cho phép null nếu không muốn đổi mật khẩu
        public string TrangThaiTK { get; set; }
    }
}