using AutoMapper;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.HoaDons;

namespace WebCoffee.BackendServer.Mappings
{
    public class HoaDonProfile : Profile
    {
        public HoaDonProfile()
        {
            CreateMap<CTHD, CTHDVm>();
            CreateMap<HoaDon, HoaDonVm>()
                .ForMember(dest => dest.ChiTietHoaDons, opt => opt.MapFrom(src => src.CTHDs));

            CreateMap<CTHDCreateRequest, CTHD>();
            CreateMap<HoaDonCreateRequest, HoaDon>()
                .ForMember(dest => dest.CTHDs, opt => opt.MapFrom(src => src.ChiTietHoaDons));

            CreateMap<HoaDonUpdateRequest, HoaDon>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}