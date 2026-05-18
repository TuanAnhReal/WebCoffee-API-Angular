using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Models;
using WebCoffee.BackendServer.Services.SanPhams;

var builder = WebApplication.CreateBuilder(args);

// Kết nối đến SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

//Thêm các Controllers (Chỉ gọi 1 lần)
builder.Services.AddControllers();

//CẤU HÌNH SWAGGER - PHẦN SERVICES
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "WebCoffee API",
        Version = "v1",
        Description = "Hệ thống API quản lý quán Cafe (QL_QUANCAFE_AK)"
    });
});

//Đăng ký kết nối Interface và Class thực thi (Service)
builder.Services.AddTransient<ISanPhamService, SanPhamService>();

//Đăng ký lớp Model đóng vai trò thư viện cho Controller
builder.Services.AddTransient<SanPhamModel>();

var app = builder.Build();

//CẤU HÌNH MIDDLEWARE (HTTP request pipeline)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebCoffee API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();