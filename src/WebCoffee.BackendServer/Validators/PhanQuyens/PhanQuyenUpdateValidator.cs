using FluentValidation;
using WebCoffee.ViewModels.System.PhanQuyens;

namespace WebCoffee.BackendServer.Validators.PhanQuyens
{
    public class PhanQuyenUpdateValidator : AbstractValidator<PhanQuyenUpdateRequest>
    {
        public PhanQuyenUpdateValidator()
        {
            RuleFor(x => x.TenPQ).NotEmpty().WithMessage("Tên phân quyền không được để trống.");
            RuleFor(x => x.MoTaPQ).NotEmpty().WithMessage("Mô tả phân quyền không được để trống.");
        }
    }
}