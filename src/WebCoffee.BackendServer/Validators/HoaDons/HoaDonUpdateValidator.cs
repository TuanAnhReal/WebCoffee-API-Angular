using FluentValidation;
using WebCoffee.ViewModels.Catalog.HoaDons;

namespace WebCoffee.BackendServer.Validators.HoaDons
{
    public class HoaDonUpdateValidator : AbstractValidator<HoaDonUpdateRequest>
    {
        public HoaDonUpdateValidator()
        {
            RuleFor(x => x.SoBan).NotEmpty().WithMessage("Số bàn không được để trống.");
            RuleFor(x => x.TGRa).GreaterThan(x => x.TGRa).WithMessage("Thời gian ra phải lớn hơn thời gian vào.");
            RuleFor(x => x.TrangThaiHD).NotEmpty().WithMessage("Trạng thái hóa đơn không được để trống.");
        }
    }
}