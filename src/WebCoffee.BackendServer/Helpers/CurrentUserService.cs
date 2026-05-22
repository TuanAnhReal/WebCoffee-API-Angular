using System.Security.Claims;

namespace WebCoffee.BackendServer.Helpers
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        public string Role => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;

        public string MaNV => _httpContextAccessor.HttpContext?.User?.FindFirst("MaNV")?.Value;

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}