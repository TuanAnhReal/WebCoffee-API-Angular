using System.Collections.Generic;
using System.Threading.Tasks;
using WebCoffee.ViewModels.Catalog.KhuVucBans;
using WebCoffee.ViewModels.Common;

namespace WebCoffee.BackendServer.Services.KhuVucBans
{
    public interface IKhuVucBanService
    {
        Task<ApiResponse<List<KhuVucVm>>> GetAllWithBansAsync();
        Task<ApiResponse<bool>> CreateKhuVucAsync(KhuVucCreateRequest request);
        Task<ApiResponse<bool>> UpdateKhuVucAsync(string soKV, KhuVucUpdateRequest request);
        Task<ApiResponse<bool>> DeleteKhuVucAsync(string soKV);
        Task<ApiResponse<bool>> CreateBanAsync(BanCreateRequest request);
        Task<ApiResponse<bool>> UpdateBanAsync(string soBan, BanUpdateRequest request);
        Task<ApiResponse<bool>> DeleteBanAsync(string soBan);
    }
}