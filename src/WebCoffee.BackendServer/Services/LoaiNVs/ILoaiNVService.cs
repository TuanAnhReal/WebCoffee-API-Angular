using WebCoffee.ViewModels.Catalog.LoaiNVs;

namespace WebCoffee.BackendServer.Services.LoaiNVs
{
    public interface ILoaiNVService
    {
        Task<List<LoaiNVVm>> GetAllAsync();
        Task<LoaiNVVm> GetByIdAsync(string maLoaiNv);
        Task<LoaiNVVm> CreateAsync(LoaiNVCreateRequest request);
        Task<bool> UpdateAsync(string maLoaiNv, LoaiNVUpdateRequest request);
        Task<bool> DeleteAsync(string maLoaiNv);
    }
}