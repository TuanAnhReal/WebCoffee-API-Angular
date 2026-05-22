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
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}