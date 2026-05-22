using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.System.PhanQuyens;

namespace WebCoffee.BackendServer.Services.PhanQuyens
{
    public class PhanQuyenService : IPhanQuyenService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PhanQuyenService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PhanQuyenVm>> GetAllAsync()
        {
            var entities = await _context.PhanQuyens.ToListAsync();
            return _mapper.Map<List<PhanQuyenVm>>(entities);
        }

        public async Task<PhanQuyenVm> GetByIdAsync(string maPq)
        {
            var entity = await _context.PhanQuyens.FindAsync(maPq);
            if (entity == null) return null;
            return _mapper.Map<PhanQuyenVm>(entity);
        }

        public async Task<PhanQuyenVm> CreateAsync(PhanQuyenCreateRequest request)
        {
            var entity = _mapper.Map<PhanQuyen>(request);

            // Tự động sinh mã phân quyền
            entity.MaPQ = "PQ" + DateTime.Now.ToString("HHmmss");

            _context.PhanQuyens.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<PhanQuyenVm>(entity);
        }

        public async Task<bool> UpdateAsync(string maPq, PhanQuyenUpdateRequest request)
        {
            var entity = await _context.PhanQuyens.FindAsync(maPq);
            if (entity == null) return false;

            // AutoMapper tự động đè các giá trị mới từ request sang entity
            _mapper.Map(request, entity);

            _context.PhanQuyens.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(string maPq)
        {
            var entity = await _context.PhanQuyens.FindAsync(maPq);
            if (entity == null) return false;

            _context.PhanQuyens.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}