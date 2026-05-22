using FluentValidation;
using WebCoffee.ViewModels.Catalog.NhanViens;

namespace WebCoffee.BackendServer.Validators.NhanViens
{
    public class NhanVienUpdateValidator : AbstractValidator<NhanVienUpdateRequest>
    {
        public NhanVienUpdateValidator()
        {
            RuleFor(x => x.MaLoaiNV).NotEmpty().WithMessage("Mã loại nhân viên không được để trống");
            RuleFor(x => x.HoNV).NotEmpty().WithMessage("Họ nhân viên không được để trống");
            RuleFor(x => x.TenNV).NotEmpty().WithMessage("Tên nhân viên không được để trống");
            RuleFor(x => x.SoDTNV).Matches(@"^(0[3|5|7|8|9])+([0-9]{8})$").When(x => !string.IsNullOrEmpty(x.SoDTNV))
                                  .WithMessage("Số điện thoại không đúng định dạng.");
        }
    }
}