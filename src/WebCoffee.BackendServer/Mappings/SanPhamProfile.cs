using AutoMapper;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.SanPhams;

namespace WebCoffee.BackendServer.Mappings
{
    public class SanPhamProfile : Profile
    {
        public SanPhamProfile()
        {
            // Entity -> ViewModel
            CreateMap<SanPham, SanPhamVm>()
                .ForMember(dest => dest.TenLoaiSp, opt => opt.MapFrom(src => src.LoaiSP.TenLoaiSP));

            // Request -> Entity
            CreateMap<SanPhamCreateRequest, SanPham>()
                .ForMember(dest => dest.TrangThaiSP, opt => opt.MapFrom(src => "Đang bán")); // Set mặc định

            CreateMap<SanPhamUpdateRequest, SanPham>();
        }
    }
}