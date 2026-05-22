using WebCoffee.ViewModels.Catalog.LoaiSPs;

namespace WebCoffee.BackendServer.Services.LoaiSPs
{
    public interface ILoaiSPService
    {
        Task<List<LoaiSPVm>> GetAllAsync();
        Task<LoaiSPVm> GetByIdAsync(string maLoaiSp);
        Task<LoaiSPVm> CreateAsync(LoaiSPCreateRequest request);
        Task<bool> UpdateAsync(string maLoaiSp, LoaiSPUpdateRequest request);
        Task<bool> DeleteAsync(string maLoaiSp);
    }
}