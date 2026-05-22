using FluentValidation;
using WebCoffee.ViewModels.System.Auth;

namespace WebCoffee.ViewModels.Validators.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.TenDangNhap)
                .NotEmpty().WithMessage("Tên đăng nhập không được để trống.");

            RuleFor(x => x.MatKhau)
                .NotEmpty().WithMessage("Mật khẩu không được để trống.")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.");
        }
    }
}