using FluentValidation;
using WebCoffee.ViewModels.Catalog.KhachHangs;

namespace WebCoffee.BackendServer.Validators.KhachHangs
{
    public class KhachHangUpdateValidator : AbstractValidator<KhachHangUpdateRequest>
    {
        public KhachHangUpdateValidator()
        {
            RuleFor(x => x.TenKH).NotEmpty().When(x => x.TenKH != null)
                                 .WithMessage("Tên khách hàng không được để chuỗi rỗng.");
            RuleFor(x => x.SDTKH).Matches(@"^(0[3|5|7|8|9])+([0-9]{8})$").When(x => !string.IsNullOrEmpty(x.SDTKH))
                                 .WithMessage("Số điện thoại không đúng định dạng.");

            RuleFor(x => x.DiemTichLuy).GreaterThanOrEqualTo(0).When(x => x.DiemTichLuy.HasValue)
                                       .WithMessage("Điểm tích lũy không được là số âm.");
        }
    }
}