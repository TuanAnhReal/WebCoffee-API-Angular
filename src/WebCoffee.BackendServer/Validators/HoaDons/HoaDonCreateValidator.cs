using FluentValidation;
using WebCoffee.ViewModels.Catalog.HoaDons;

namespace WebCoffee.BackendServer.Validators.HoaDons
{
    public class HoaDonCreateValidator : AbstractValidator<HoaDonCreateRequest>
    {
        public HoaDonCreateValidator()
        {
            RuleFor(x => x.ChiTietHoaDons)
                .NotEmpty().WithMessage("Hóa đơn phải có ít nhất 1 sản phẩm.");

            RuleForEach(x => x.ChiTietHoaDons).SetValidator(new CTHDCreateValidator());
        }
    }

    public class CTHDCreateValidator : AbstractValidator<CTHDCreateRequest>
    {
        public CTHDCreateValidator()
        {
            RuleFor(x => x.MaSP).NotEmpty().WithMessage("Mã sản phẩm không được trống.");
            RuleFor(x => x.SLSP).GreaterThan(0).WithMessage("Số lượng sản phẩm phải lớn hơn 0.");
            RuleFor(x => x.DonGia).GreaterThanOrEqualTo(0).WithMessage("Đơn giá không hợp lệ.");
        }
    }
}