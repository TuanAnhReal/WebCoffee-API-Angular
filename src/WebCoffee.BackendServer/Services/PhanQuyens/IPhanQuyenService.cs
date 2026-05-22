using WebCoffee.ViewModels.System.PhanQuyens;

namespace WebCoffee.BackendServer.Services.PhanQuyens
{
    public interface IPhanQuyenService
    {
        Task<List<PhanQuyenVm>> GetAllAsync();
        Task<PhanQuyenVm> GetByIdAsync(string maPq);
        Task<PhanQuyenVm> CreateAsync(PhanQuyenCreateRequest request);
        Task<bool> UpdateAsync(string maPq, PhanQuyenUpdateRequest request);
        Task<bool> DeleteAsync(string maPq);
    }
}