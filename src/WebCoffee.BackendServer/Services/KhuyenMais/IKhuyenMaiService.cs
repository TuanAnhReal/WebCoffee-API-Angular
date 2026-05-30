using System.Collections.Generic;
using System.Threading.Tasks;
using WebCoffee.ViewModels.Catalog.KhuyenMais;
using WebCoffee.ViewModels.Common;

namespace WebCoffee.BackendServer.Services.KhuyenMais
{
    public interface IKhuyenMaiService
    {
        Task<ServiceResult<List<KhuyenMaiVm>>> GetAllAsync();
        Task<ServiceResult<KhuyenMaiVm>> GetByIdAsync(string maKm);
        Task<ServiceResult<KhuyenMaiVm>> CreateAsync(KhuyenMaiCreateRequest request);
        Task<ServiceResult> UpdateAsync(string maKm, KhuyenMaiUpdateRequest request);
        Task<ServiceResult> DeleteAsync(string maKm);

        Task<ServiceResult> GanSanPhamAsync(string maKm, GanSanPhamKhuyenMaiRequest request);
        Task<ServiceResult> XoaSanPhamAsync(string maKm, string maSp);
        Task<ServiceResult<List<KhuyenMaiSanPhamVm>>> GetSanPhamsByKhuyenMaiAsync(string maKm);
    }
}