using AutoMapper;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.KhachHangs;

namespace WebCoffee.BackendServer.Mappings
{
    public class KhachHangProfile : Profile
    {
        public KhachHangProfile()
        {
            CreateMap<KhachHang, KhachHangVm>();

            CreateMap<KhachHangCreateRequest, KhachHang>();

            CreateMap<KhachHangUpdateRequest, KhachHang>()
                .ForMember(dest => dest.NgayTao, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}