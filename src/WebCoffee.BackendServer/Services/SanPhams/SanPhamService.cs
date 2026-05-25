using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.BackendServer.Services.Storage;
using WebCoffee.ViewModels.Catalog.SanPhams;

namespace WebCoffee.BackendServer.Services.SanPhams
{
    public class SanPhamService : ISanPhamService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;

        public SanPhamService(AppDbContext context, IMapper mapper, IStorageService storageService)
        {
            _context = context;
            _mapper = mapper;
            _storageService = storageService;
        }

        public async Task<List<SanPhamVm>> GetAllAsync()
        {
            var entities = await _context.SanPhams.Include(x => x.LoaiSP).ToListAsync();
            return _mapper.Map<List<SanPhamVm>>(entities);
        }

        public async Task<SanPhamVm> GetByIdAsync(string maSp)
        {
            var entity = await _context.SanPhams.Include(x => x.LoaiSP).FirstOrDefaultAsync(x => x.MaSP == maSp);
            if (entity == null) return null;
            return _mapper.Map<SanPhamVm>(entity);
        }

        public async Task<SanPhamVm> CreateAsync(SanPhamCreateRequest request)
        {
            var entity = _mapper.Map<SanPham>(request);
            entity.MaSP = "SP" + DateTime.Now.ToString("HHmmss");
            entity.TrangThaiSP = "Đang bán";

            if (request.HinhAnhFile != null)
            {
                entity.HinhAnh = await _storageService.UploadImageAsync(request.HinhAnhFile, "SanPhams");
            }

            _context.SanPhams.Add(entity);

            await _context.SaveChangesAsync();
            await _context.Entry(entity).Reference(x => x.LoaiSP).LoadAsync();

            return _mapper.Map<SanPhamVm>(entity);
        }

        public async Task<bool> UpdateAsync(string maSp, SanPhamUpdateRequest request)
        {
            var entity = await _context.SanPhams.FirstOrDefaultAsync(x => x.MaSP == maSp);
            if (entity == null) return false;

            // Nếu người dùng chọn ảnh mới
            if (request.HinhAnhFile != null)
            {
                // Xóa ảnh cũ trên Cloudinary (nếu có)
                if (!string.IsNullOrEmpty(entity.HinhAnh))
                {
                    await _storageService.DeleteImageAsync(entity.HinhAnh);
                }

                // Upload ảnh mới lên Cloudinary và lấy URL gán vào DB
                entity.HinhAnh = await _storageService.UploadImageAsync(request.HinhAnhFile, "SanPhams");
            }

            _mapper.Map(request, entity);
            _context.SanPhams.Update(entity);

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(string maSp)
        {
            var entity = await _context.SanPhams.FirstOrDefaultAsync(x => x.MaSP == maSp);
            if (entity == null) return false;

            if (!string.IsNullOrEmpty(entity.HinhAnh))
            {
                await _storageService.DeleteImageAsync(entity.HinhAnh);
            }

            _context.SanPhams.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}