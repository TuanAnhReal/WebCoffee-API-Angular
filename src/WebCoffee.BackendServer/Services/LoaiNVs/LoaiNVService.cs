using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.LoaiNVs;

namespace WebCoffee.BackendServer.Services.LoaiNVs
{
    public class LoaiNVService : ILoaiNVService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public LoaiNVService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<LoaiNVVm>> GetAllAsync()
        {
            var entities = await _context.LoaiNVs.ToListAsync();
            return _mapper.Map<List<LoaiNVVm>>(entities);
        }

        public async Task<LoaiNVVm> GetByIdAsync(string maLoaiNv)
        {
            var entity = await _context.LoaiNVs.FindAsync(maLoaiNv);
            if (entity == null) return null;
            return _mapper.Map<LoaiNVVm>(entity);
        }

        public async Task<LoaiNVVm> CreateAsync(LoaiNVCreateRequest request)
        {
            var entity = _mapper.Map<LoaiNV>(request);

            // Tự động sinh mã
            entity.MaLoaiNV = "LNV" + DateTime.Now.ToString("HHmmss");

            _context.LoaiNVs.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<LoaiNVVm>(entity);
        }

        public async Task<bool> UpdateAsync(string maLoaiNv, LoaiNVUpdateRequest request)
        {
            var entity = await _context.LoaiNVs.FindAsync(maLoaiNv);
            if (entity == null) return false;

            // AutoMapper ánh xạ tự động, đè dữ liệu mới, bỏ qua null
            _mapper.Map(request, entity);

            _context.LoaiNVs.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(string maLoaiNv)
        {
            var entity = await _context.LoaiNVs.FindAsync(maLoaiNv);
            if (entity == null) return false;

            _context.LoaiNVs.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}