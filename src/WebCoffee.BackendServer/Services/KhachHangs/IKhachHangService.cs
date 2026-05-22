using WebCoffee.ViewModels.Catalog.KhachHangs;

namespace WebCoffee.BackendServer.Services.KhachHangs
{
    public interface IKhachHangService
    {
        Task<List<KhachHangVm>> GetAllAsync();
        Task<KhachHangVm> GetByIdAsync(string maKh);
        Task<KhachHangVm> CreateAsync(KhachHangCreateRequest request);
        Task<bool> UpdateAsync(string maKh, KhachHangUpdateRequest request);
        Task<bool> DeleteAsync(string maKh);
    }
}