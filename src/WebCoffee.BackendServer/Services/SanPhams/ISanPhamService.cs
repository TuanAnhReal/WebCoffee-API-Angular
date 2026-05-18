using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog;

namespace WebCoffee.BackendServer.Services.SanPhams
{
    public interface ISanPhamService
    {
        Task<List<SanPhamVm>> GetAllAsync();
        Task<SanPhamVm> GetByIdAsync(string maSp);
        Task<string> CreateAsync(SanPhamCreateRequest request);
        Task<int> UpdateAsync(string maSp, SanPhamUpdateRequest request);
        Task<int> DeleteAsync(string maSp);
    }
}
