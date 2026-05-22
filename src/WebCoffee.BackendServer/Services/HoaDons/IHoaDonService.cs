using WebCoffee.ViewModels.Catalog.HoaDons;

namespace WebCoffee.BackendServer.Services.HoaDons
{
    public interface IHoaDonService
    {
        Task<List<HoaDonVm>> GetAllAsync();
        Task<HoaDonVm> GetByIdAsync(string soHd);
        Task<HoaDonVm> CreateAsync(HoaDonCreateRequest request);
        Task<bool> UpdateAsync(string soHd, HoaDonUpdateRequest request);
        Task<bool> DeleteAsync(string soHd);
    }
}