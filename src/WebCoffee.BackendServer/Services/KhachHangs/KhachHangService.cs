using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.KhachHangs;

namespace WebCoffee.BackendServer.Services.KhachHangs
{
    public class KhachHangService : IKhachHangService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public KhachHangService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<KhachHangVm>> GetAllAsync()
        {
            var entities = await _context.KhachHangs.ToListAsync();
            return _mapper.Map<List<KhachHangVm>>(entities);
        }

        public async Task<KhachHangVm> GetByIdAsync(string maKh)
        {
            var entity = await _context.KhachHangs.FindAsync(maKh);
            if (entity == null) return null;
            return _mapper.Map<KhachHangVm>(entity);
        }

        public async Task<KhachHangVm> CreateAsync(KhachHangCreateRequest request)
        {
            var entity = _mapper.Map<KhachHang>(request);

            // Tự động sinh mã & gán giá trị mặc định theo nghiệp vụ cũ
            entity.MaKH = "KH" + DateTime.Now.ToString("HHmmss");
            entity.TenKH = string.IsNullOrWhiteSpace(request.TenKH) ? "Khách vãng lai" : request.TenKH;
            entity.DiemTichLuy = request.DiemTichLuy ?? 0;
            entity.NgayTao = DateTime.Now;

            _context.KhachHangs.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<KhachHangVm>(entity);
        }

        public async Task<bool> UpdateAsync(string maKh, KhachHangUpdateRequest request)
        {
            var entity = await _context.KhachHangs.FindAsync(maKh);
            if (entity == null) return false;

            // AutoMapper tự động ánh xạ, bỏ qua trường null
            _mapper.Map(request, entity);

            _context.KhachHangs.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(string maKh)
        {
            var entity = await _context.KhachHangs.FindAsync(maKh);
            if (entity == null) return false;

            _context.KhachHangs.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}