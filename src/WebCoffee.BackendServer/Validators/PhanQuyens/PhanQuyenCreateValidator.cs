using FluentValidation;
using WebCoffee.ViewModels.System.PhanQuyens;

namespace WebCoffee.BackendServer.Validators.PhanQuyens
{
    public class PhanQuyenCreateValidator : AbstractValidator<PhanQuyenCreateRequest>
    {
        public PhanQuyenCreateValidator()
        {
            RuleFor(x => x.TenPQ).NotEmpty().WithMessage("Tên phân quyền không được để trống.");
        }
    }
}