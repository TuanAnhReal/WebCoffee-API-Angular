using FluentValidation;
using WebCoffee.ViewModels.Catalog.KhuyenMais;

namespace WebCoffee.BackendServer.Validators.KhuyenMais
{
    public class KhuyenMaiCreateValidator : AbstractValidator<KhuyenMaiCreateRequest>
    {
        public KhuyenMaiCreateValidator()
        {
            RuleFor(x => x.TenKM)
                .NotEmpty().WithMessage("Tên khuyến mãi không được để trống.");

            RuleFor(x => x.LoaiKM)
                .NotEmpty().WithMessage("Loại khuyến mãi không được để trống.")
                .Must(x => x == "PERCENT" || x == "AMOUNT")
                .WithMessage("Loại khuyến mãi chỉ được là PERCENT hoặc AMOUNT.");

            RuleFor(x => x.GiaTriKM)
                .NotNull().WithMessage("Giá trị khuyến mãi là bắt buộc.")
                .GreaterThan(0).WithMessage("Giá trị khuyến mãi phải lớn hơn 0.")
                .LessThanOrEqualTo(100).When(x => x.LoaiKM == "PERCENT")
                .WithMessage("Khuyến mãi phần trăm không được vượt quá 100%.");

            RuleFor(x => x.NgayBD)
                .NotNull().WithMessage("Ngày bắt đầu là bắt buộc.");

            RuleFor(x => x.NgayKT)
                .NotNull().WithMessage("Ngày kết thúc là bắt buộc.");

            RuleFor(x => x)
                .Must(x => x.NgayKT >= x.NgayBD)
                .When(x => x.NgayBD != null && x.NgayKT != null)
                .WithMessage("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
        }
    }
}