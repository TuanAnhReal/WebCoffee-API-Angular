using FluentValidation;
using WebCoffee.ViewModels.Catalog.NhanViens;

namespace WebCoffee.BackendServer.Validators.NhanViens
{
    public class NhanVienCreateValidator : AbstractValidator<NhanVienCreateRequest>
    {
        public NhanVienCreateValidator()
        {
            RuleFor(x => x.HoNV).NotEmpty().WithMessage("Họ nhân viên không được để trống.");
            RuleFor(x => x.TenNV).NotEmpty().WithMessage("Tên nhân viên không được để trống.");
            RuleFor(x => x.MaLoaiNV).NotEmpty().WithMessage("Vui lòng chọn loại nhân viên.");
            RuleFor(x => x.SoDTNV).Matches(@"^(0[3|5|7|8|9])+([0-9]{8})$").When(x => !string.IsNullOrEmpty(x.SoDTNV))
                                  .WithMessage("Số điện thoại không đúng định dạng.");
        }
    }
}