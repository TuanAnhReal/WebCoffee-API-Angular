using FluentValidation;
using WebCoffee.ViewModels.System.TaiKhoans;

namespace WebCoffee.BackendServer.Validators.TaiKhoans
{
    public class TaiKhoanUpdateValidator : AbstractValidator<TaiKhoanUpdateRequest>
    {
        public TaiKhoanUpdateValidator()
        {
            RuleFor(x => x.MaNV).NotEmpty().WithMessage("Mã nhân viên không được để trống");
            RuleFor(x => x.MaPQ).NotEmpty().WithMessage("Mã phân quyền không được để trống");
            RuleFor(x => x.MatKhau).MinimumLength(6).WithMessage("Mật khẩu phải từ 6 ký tự");
        }
    }
}