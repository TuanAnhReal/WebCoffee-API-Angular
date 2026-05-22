using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.System.TaiKhoans;

namespace WebCoffee.BackendServer.Services.TaiKhoans
{
    public class TaiKhoanService : ITaiKhoanService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TaiKhoanService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TaiKhoanVm>> GetAllAsync()
        {
            var entities = await _context.TaiKhoans.ToListAsync();
            return _mapper.Map<List<TaiKhoanVm>>(entities);
        }

        public async Task<TaiKhoanVm> GetByIdAsync(string tenDangNhap)
        {
            var entity = await _context.TaiKhoans.FirstOrDefaultAsync(x => x.TenDangNhap == tenDangNhap);
            if (entity == null) return null;
            return _mapper.Map<TaiKhoanVm>(entity);
        }

        public async Task<TaiKhoanVm> CreateAsync(TaiKhoanCreateRequest request)
        {
            var exists = await _context.TaiKhoans.AnyAsync(x => x.TenDangNhap == request.TenDangNhap);
            if (exists) return null;

            var entity = _mapper.Map<TaiKhoan>(request);

            entity.MatKhau = BCrypt.Net.BCrypt.HashPassword(request.MatKhau);
            entity.TrangThaiTK = request.TrangThaiTK ?? "Hoạt động";

            _context.TaiKhoans.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TaiKhoanVm>(entity);
        }

        public async Task<bool> UpdateAsync(string tenDangNhap, TaiKhoanUpdateRequest request)
        {
            var entity = await _context.TaiKhoans.FirstOrDefaultAsync(x => x.TenDangNhap == tenDangNhap);
            if (entity == null) return false;

            _mapper.Map(request, entity);

            if (!string.IsNullOrEmpty(request.MatKhau))
            {
                entity.MatKhau = BCrypt.Net.BCrypt.HashPassword(request.MatKhau);
            }

            _context.TaiKhoans.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(string tenDangNhap)
        {
            var entity = await _context.TaiKhoans.FirstOrDefaultAsync(x => x.TenDangNhap == tenDangNhap);
            if (entity == null) return false;

            _context.TaiKhoans.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}