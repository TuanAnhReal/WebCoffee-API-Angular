using AutoMapper;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.System.PhanQuyens;

namespace WebCoffee.BackendServer.Mappings
{
    public class PhanQuyenProfile : Profile
    {
        public PhanQuyenProfile()
        {
            CreateMap<PhanQuyen, PhanQuyenVm>();
            CreateMap<PhanQuyenCreateRequest, PhanQuyen>();
            CreateMap<PhanQuyenUpdateRequest, PhanQuyen>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}