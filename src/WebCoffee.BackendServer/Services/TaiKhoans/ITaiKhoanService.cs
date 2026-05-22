using WebCoffee.ViewModels.System.TaiKhoans;

namespace WebCoffee.BackendServer.Services.TaiKhoans
{
    public interface ITaiKhoanService
    {
        Task<List<TaiKhoanVm>> GetAllAsync();
        Task<TaiKhoanVm> GetByIdAsync(string tenDangNhap);
        Task<TaiKhoanVm> CreateAsync(TaiKhoanCreateRequest request);
        Task<bool> UpdateAsync(string tenDangNhap, TaiKhoanUpdateRequest request);
        Task<bool> DeleteAsync(string tenDangNhap);
    }
}