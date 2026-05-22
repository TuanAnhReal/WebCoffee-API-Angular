using AutoMapper;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.System.TaiKhoans;

namespace WebCoffee.BackendServer.Mappings
{
    public class TaiKhoanProfile : Profile
    {
        public TaiKhoanProfile()
        {
            CreateMap<TaiKhoan, TaiKhoanVm>();
            CreateMap<TaiKhoanCreateRequest, TaiKhoan>()
                .ForMember(dest => dest.MatKhau, opt => opt.Ignore());

            CreateMap<TaiKhoanUpdateRequest, TaiKhoan>()
                .ForMember(dest => dest.MatKhau, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}