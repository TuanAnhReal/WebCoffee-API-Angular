using AutoMapper;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.KhuyenMais;

namespace WebCoffee.BackendServer.Mappings
{
    public class KhuyenMaiProfile : Profile
    {
        public KhuyenMaiProfile()
        {
            CreateMap<KhuyenMai, KhuyenMaiVm>();

            CreateMap<KhuyenMaiCreateRequest, KhuyenMai>()
                .ForMember(dest => dest.TrangThaiKM, opt => opt.Ignore());

            CreateMap<KhuyenMaiUpdateRequest, KhuyenMai>()
                .ForMember(dest => dest.TrangThaiKM, opt => opt.Ignore());
        }
    }
}