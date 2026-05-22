using AutoMapper;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.LoaiSPs;

namespace WebCoffee.BackendServer.Mappings
{
    public class LoaiSPProfile : Profile
    {
        public LoaiSPProfile()
        {
            CreateMap<LoaiSP, LoaiSPVm>();

            CreateMap<LoaiSPCreateRequest, LoaiSP>();

            CreateMap<LoaiSPUpdateRequest, LoaiSP>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}