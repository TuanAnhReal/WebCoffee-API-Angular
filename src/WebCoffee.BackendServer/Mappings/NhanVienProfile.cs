using AutoMapper;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.NhanViens;

namespace WebCoffee.BackendServer.Mappings
{
    public class NhanVienProfile : Profile
    {
        public NhanVienProfile()
        {
            CreateMap<NhanVien, NhanVienVm>();
            CreateMap<NhanVienCreateRequest, NhanVien>();
            CreateMap<NhanVienUpdateRequest, NhanVien>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}