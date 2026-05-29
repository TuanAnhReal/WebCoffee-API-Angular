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
            var tenDangNhapTonTai = await _context.TaiKhoans.AnyAsync(x => x.TenDangNhap == request.TenDangNhap);
            if (tenDangNhapTonTai) return null;

            var nhanVienTonTai = await _context.NhanViens.AnyAsync(x => x.MaNV == request.MaNV);
            if (!nhanVienTonTai) throw new Exception("Mã nhân viên không tồn tại trên hệ thống danh mục.");

            var nhanVienDaCoTaiKhoan = await _context.TaiKhoans.AnyAsync(x => x.MaNV == request.MaNV);
            if (nhanVienDaCoTaiKhoan) throw new Exception("Nhân viên này đã được cấu hình tài khoản truy cập từ trước.");

            var phanQuyenTonTai = await _context.PhanQuyens.AnyAsync(x => x.MaPQ == request.MaPQ);
            if (!phanQuyenTonTai) throw new Exception("Mã phân quyền không hợp lệ hoặc không tồn tại.");

            var entity = _mapper.Map<TaiKhoan>(request);

            entity.MatKhau = BCrypt.Net.BCrypt.HashPassword(request.MatKhau);
            entity.TrangThaiTK = request.TrangThaiTK ?? "Hoạt động";
            entity.MaNV = request.MaNV;

            entity.MaPQ = request.MaPQ;

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