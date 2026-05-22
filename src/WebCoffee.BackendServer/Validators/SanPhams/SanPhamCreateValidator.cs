using FluentValidation;
using WebCoffee.ViewModels.Catalog.SanPhams;

namespace WebCoffee.BackendServer.Validators.SanPhams
{
    public class SanPhamCreateValidator : AbstractValidator<SanPhamCreateRequest>
    {
        public SanPhamCreateValidator()
        {
            RuleFor(x => x.MaSp).NotEmpty().WithMessage("Mã sản phẩm không được để trống");
            RuleFor(x => x.TenSp).NotEmpty().WithMessage("Tên sản phẩm không được để trống");
            RuleFor(x => x.GiaSp).GreaterThan(0).WithMessage("Giá sản phẩm phải lớn hơn 0");
            RuleFor(x => x.MaLoaiSp).NotEmpty().WithMessage("Vui lòng chọn loại sản phẩm");
        }
    }
}