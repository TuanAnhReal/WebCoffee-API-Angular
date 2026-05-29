using WebCoffee.ViewModels.System.Auth;

namespace WebCoffee.BackendServer.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResponse> AuthenticateAsync(LoginRequest request);
        Task<LoginResponse> RenewTokenAsync(RefreshTokenRequest request);
        Task<bool> RevokeTokenAsync(string username);
        Task<bool> ChangePasswordAsync(string username, ChangePasswordRequest request);
        Task<bool> DangKyTaiKhoanAsync(DangKyRequest request);
    }
}