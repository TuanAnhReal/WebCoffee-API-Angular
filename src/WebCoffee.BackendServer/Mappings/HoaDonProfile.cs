using AutoMapper;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.HoaDons;

namespace WebCoffee.BackendServer.Mappings
{
    public class HoaDonProfile : Profile
    {
        public HoaDonProfile()
        {
            CreateMap<CTHD, CTHDVm>()
                .ForMember(dest => dest.TenSP, opt => opt.MapFrom(src => src.SanPham != null ? src.SanPham.TenSP : string.Empty))
                .ForMember(dest => dest.CoKhuyenMai, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.MaKM)))
                .ForMember(dest => dest.MaKhuyenMai, opt => opt.MapFrom(src => src.MaKM))
                .ForMember(dest => dest.TenKhuyenMai, opt => opt.MapFrom(src => src.TenKM))
                .ForMember(dest => dest.LoaiKhuyenMai, opt => opt.MapFrom(src => src.LoaiKM))
                .ForMember(dest => dest.GiaTriKhuyenMai, opt => opt.MapFrom(src => src.GiaTriKM));

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