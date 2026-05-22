using FluentValidation;
using WebCoffee.ViewModels.System.TaiKhoans;

namespace WebCoffee.BackendServer.Validators.TaiKhoans
{
    public class TaiKhoanCreateValidator : AbstractValidator<TaiKhoanCreateRequest>
    {
        public TaiKhoanCreateValidator()
        {
            RuleFor(x => x.TenDangNhap).NotEmpty().WithMessage("Tên đăng nhập không được để trống");
            RuleFor(x => x.MatKhau).NotEmpty().WithMessage("Mật khẩu không được để trống")
                                   .MinimumLength(6).WithMessage("Mật khẩu phải từ 6 ký tự");
            RuleFor(x => x.MaNV).NotEmpty().WithMessage("Mã nhân viên không được để trống");
            RuleFor(x => x.MaPQ).NotEmpty().WithMessage("Mã phân quyền không được để trống");
        }
    }
}