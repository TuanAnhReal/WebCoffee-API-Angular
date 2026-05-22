using AutoMapper;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.LoaiNVs;

namespace WebCoffee.BackendServer.Mappings
{
    public class LoaiNVProfile : Profile
    {
        public LoaiNVProfile()
        {
            CreateMap<LoaiNV, LoaiNVVm>();

            CreateMap<LoaiNVCreateRequest, LoaiNV>();

            CreateMap<LoaiNVUpdateRequest, LoaiNV>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}