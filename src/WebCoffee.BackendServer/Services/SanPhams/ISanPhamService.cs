using WebCoffee.ViewModels.Catalog.SanPhams;

namespace WebCoffee.BackendServer.Services.SanPhams
{
    public interface ISanPhamService
    {
        Task<List<SanPhamVm>> GetAllAsync();
        Task<SanPhamVm> GetByIdAsync(string maSp);
        Task<SanPhamVm> CreateAsync(SanPhamCreateRequest request);
        Task<bool> UpdateAsync(string maSp, SanPhamUpdateRequest request);
        Task<bool> DeleteAsync(string maSp);
    }
}