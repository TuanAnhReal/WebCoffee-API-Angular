using AutoMapper;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.SanPhams;

namespace WebCoffee.BackendServer.Mappings
{
    public class SanPhamProfile : Profile
    {
        public SanPhamProfile()
        {
            CreateMap<SanPham, SanPhamVm>()
                .ForMember(dest => dest.TenLoaiSp, opt => opt.MapFrom(src => src.LoaiSP.TenLoaiSP))
                .ForMember(dest => dest.GiaSp, opt => opt.MapFrom(src => src.DonGia))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThaiSP));

            CreateMap<SanPhamCreateRequest, SanPham>()
                .ForMember(dest => dest.TrangThaiSP, opt => opt.MapFrom(src => src.TrangThai))
                .ForMember(dest => dest.HinhAnh, opt => opt.Ignore());

            CreateMap<SanPhamUpdateRequest, SanPham>()
                .ForMember(dest => dest.TrangThaiSP, opt => opt.MapFrom(src => src.TrangThai))
                .ForMember(dest => dest.HinhAnh, opt => opt.Ignore());
        }
    }
}