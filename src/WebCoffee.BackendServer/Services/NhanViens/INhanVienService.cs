using WebCoffee.ViewModels.Catalog.NhanViens;

namespace WebCoffee.BackendServer.Services.NhanViens
{
    public interface INhanVienService
    {
        Task<List<NhanVienVm>> GetAllAsync();
        Task<NhanVienVm> GetByIdAsync(string maNv);
        Task<NhanVienVm> CreateAsync(NhanVienCreateRequest request);
        Task<bool> UpdateAsync(string maNv, NhanVienUpdateRequest request);
        Task<bool> DeleteAsync(string maNv);
    }
}