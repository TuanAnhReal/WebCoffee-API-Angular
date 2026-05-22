using System.Security.Claims;
using WebCoffee.BackendServer.Data.Entities;

namespace WebCoffee.BackendServer.Services.Auth
{
    public interface ITokenService
    {
        string GenerateAccessToken(TaiKhoan account, string roleName);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}