using System.Collections.Generic;
using System.Threading.Tasks;
using WebCoffee.ViewModels.Catalog.CaLams;

namespace WebCoffee.BackendServer.Services.CaLams
{
    public interface ICaLamService
    {
        Task<List<CaLamVm>> GetAllAsync();
        Task<bool> CreateAsync(CaLamVm request);
    }
}