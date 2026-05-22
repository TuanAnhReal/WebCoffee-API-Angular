using FluentValidation;
using WebCoffee.ViewModels.Catalog.LoaiSPs;

namespace WebCoffee.BackendServer.Validators.LoaiSPs
{
    public class LoaiSPUpdateValidator : AbstractValidator<LoaiSPUpdateRequest>
    {
        public LoaiSPUpdateValidator()
        {
            RuleFor(x => x.TenLoaiSP).NotEmpty().WithMessage("Tên loại sản phẩm không được để trống.");
        }
    }
}