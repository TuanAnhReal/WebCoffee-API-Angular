using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.BackendServer.Services.Storage;
using WebCoffee.ViewModels.Catalog.NhanViens;

namespace WebCoffee.BackendServer.Services.NhanViens
{
    public class NhanVienService : INhanVienService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;

        public NhanVienService(AppDbContext context, IMapper mapper, IStorageService storageService)
        {
            _context = context;
            _mapper = mapper;
            _storageService = storageService;
        }

        public async Task<List<NhanVienVm>> GetAllAsync()
        {
            var entities = await _context.NhanViens.ToListAsync();
            return _mapper.Map<List<NhanVienVm>>(entities);
        }

        public async Task<NhanVienVm> GetByIdAsync(string maNv)
        {
            var entity = await _context.NhanViens.FindAsync(maNv);
            if (entity == null) return null;
            return _mapper.Map<NhanVienVm>(entity);
        }

        public async Task<NhanVienVm> CreateAsync(NhanVienCreateRequest request)
        {
            var entity = _mapper.Map<NhanVien>(request);

            entity.MaNV = "NV" + DateTime.Now.ToString("HHmmss");
            entity.TrangThaiNV = request.TrangThaiNV ?? "Đang làm việc";

            if (request.HinhAnhNV != null && request.HinhAnhNV.Length > 0)
            {
                entity.HinhAnhNV = await _storageService.UploadImageAsync(request.HinhAnhNV, "NhanViens");
            }

            _context.NhanViens.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<NhanVienVm>(entity);
        }

        public async Task<bool> UpdateAsync(string maNv, NhanVienUpdateRequest request)
        {
            var entity = await _context.NhanViens.FindAsync(maNv);
            if (entity == null) return false;

            if (request.HinhAnhNV != null && request.HinhAnhNV.Length > 0)
            {
                if (!string.IsNullOrEmpty(entity.HinhAnhNV) && entity.HinhAnhNV.StartsWith("http"))
                {
                    await _storageService.DeleteImageAsync(entity.HinhAnhNV);
                }
                entity.HinhAnhNV = await _storageService.UploadImageAsync(request.HinhAnhNV, "NhanViens");
            }

            _mapper.Map(request, entity);
            _context.NhanViens.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(string maNv)
        {
            var entity = await _context.NhanViens.FindAsync(maNv);
            if (entity == null) return false;

            _context.NhanViens.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}