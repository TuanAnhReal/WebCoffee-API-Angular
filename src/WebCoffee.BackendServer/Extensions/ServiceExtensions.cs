using Microsoft.Extensions.Configuration;
using WebCoffee.BackendServer.Helpers;
using WebCoffee.BackendServer.Services.Auth;
using WebCoffee.BackendServer.Services.BackgroundJobs;
using WebCoffee.BackendServer.Services.CaLams;
using WebCoffee.BackendServer.Services.ChamCongs;
using WebCoffee.BackendServer.Services.Dashboard;
using WebCoffee.BackendServer.Services.HoaDons;
using WebCoffee.BackendServer.Services.KhachHangs;
using WebCoffee.BackendServer.Services.KhuVucBans;
using WebCoffee.BackendServer.Services.LoaiNVs;
using WebCoffee.BackendServer.Services.LoaiSPs;
using WebCoffee.BackendServer.Services.NhanViens;
using WebCoffee.BackendServer.Services.PhanQuyens;
using WebCoffee.BackendServer.Services.SanPhams;
using WebCoffee.BackendServer.Services.Storage;
using WebCoffee.BackendServer.Services.TaiKhoans;
using WebCoffee.BackendServer.Services.KhuyenMais;

namespace WebCoffee.BackendServer.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddScoped<ICaLamService, CaLamService>();
            services.AddScoped<IChamCongService, ChamCongService>();
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
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IKhuVucBanService, KhuVucBanService>();
            services.AddScoped<IKhuyenMaiService, KhuyenMaiService>();

            services.Configure<CloudinarySettings>(
                configuration.GetSection("CloudinarySettings"));

            services.AddScoped<IStorageService, CloudinaryStorageService>();

            return services;
        }
    }
}