using FluentValidation;
using WebCoffee.ViewModels.Catalog.LoaiNVs;

namespace WebCoffee.BackendServer.Validators.LoaiNVs
{
    public class LoaiNVUpdateValidator : AbstractValidator<LoaiNVUpdateRequest>
    {
        public LoaiNVUpdateValidator()
        {
            RuleFor(x => x.TenLoaiNV).NotEmpty().WithMessage("Tên loại nhân viên không được để trống.");
        }
    }
}