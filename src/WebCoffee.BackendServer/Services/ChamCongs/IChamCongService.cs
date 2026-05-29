using System.Collections.Generic;
using System.Threading.Tasks;
using WebCoffee.ViewModels.Catalog.ChamCongs;

namespace WebCoffee.BackendServer.Services.ChamCongs
{
    public interface IChamCongService
    {
        Task<List<ChamCongVm>> GetAllAsync();
        Task<bool> CheckInAsync(ChamCongVm request);
    }
}