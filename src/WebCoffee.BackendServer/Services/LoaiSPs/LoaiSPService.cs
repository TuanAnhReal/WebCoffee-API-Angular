using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.LoaiSPs;

namespace WebCoffee.BackendServer.Services.LoaiSPs
{
    public class LoaiSPService : ILoaiSPService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public LoaiSPService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<LoaiSPVm>> GetAllAsync()
        {
            var entities = await _context.LoaiSPs.ToListAsync();
            return _mapper.Map<List<LoaiSPVm>>(entities);
        }

        public async Task<LoaiSPVm> GetByIdAsync(string maLoaiSp)
        {
            var entity = await _context.LoaiSPs.FindAsync(maLoaiSp);
            if (entity == null) return null;
            return _mapper.Map<LoaiSPVm>(entity);
        }

        public async Task<LoaiSPVm> CreateAsync(LoaiSPCreateRequest request)
        {
            var entity = _mapper.Map<LoaiSP>(request);

            entity.MaLoaiSP = "LSP" + DateTime.Now.ToString("HHmmss");

            _context.LoaiSPs.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<LoaiSPVm>(entity);
        }

        public async Task<bool> UpdateAsync(string maLoaiSp, LoaiSPUpdateRequest request)
        {
            var entity = await _context.LoaiSPs.FindAsync(maLoaiSp);
            if (entity == null) return false;

            _mapper.Map(request, entity);

            _context.LoaiSPs.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(string maLoaiSp)
        {
            var entity = await _context.LoaiSPs.FindAsync(maLoaiSp);
            if (entity == null) return false;

            _context.LoaiSPs.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}