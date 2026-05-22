using WebCoffee.BackendServer.Services.BackgroundJobs;
using WebCoffee.BackendServer.Services.Auth;
using WebCoffee.BackendServer.Helpers;
using WebCoffee.BackendServer.Services.HoaDons;
using WebCoffee.BackendServer.Services.KhachHangs;
using WebCoffee.BackendServer.Services.LoaiNVs;
using WebCoffee.BackendServer.Services.LoaiSPs;
using WebCoffee.BackendServer.Services.NhanViens;
using WebCoffee.BackendServer.Services.PhanQuyens;
using WebCoffee.BackendServer.Services.SanPhams;
using WebCoffee.BackendServer.Services.TaiKhoans;

namespace WebCoffee.BackendServer.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddTransient<ISanPhamService, SanPhamService>();
            services.AddHostedService<TokenCleanupService>();
            services.AddTransient<ILoaiSPService, LoaiSPService>();
            services.AddTransient<IKhachHangService, KhachHangService>();
            services.AddTransient<ILoaiNVService, LoaiNVService>();
            services.AddTransient<INhanVienService, NhanVienService>();
            services.AddTransient<IPhanQuyenService, PhanQuyenService>();
            services.AddTransient<ITaiKhoanService, TaiKhoanService>();
            services.AddTransient<IHoaDonService, HoaDonService>();

            return services;
        }
    }
}