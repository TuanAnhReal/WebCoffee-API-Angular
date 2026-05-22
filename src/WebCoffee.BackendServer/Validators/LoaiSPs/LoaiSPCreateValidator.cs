using FluentValidation;
using WebCoffee.ViewModels.Catalog.LoaiSPs;

namespace WebCoffee.BackendServer.Validators.LoaiSPs
{
    public class LoaiSPCreateValidator : AbstractValidator<LoaiSPCreateRequest>
    {
        public LoaiSPCreateValidator()
        {
            RuleFor(x => x.TenLoaiSP).NotEmpty().WithMessage("Tên loại sản phẩm không được để trống.");
        }
    }
}