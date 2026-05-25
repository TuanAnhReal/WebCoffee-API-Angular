using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.HoaDons;

namespace WebCoffee.BackendServer.Services.HoaDons
{
    public class HoaDonService : IHoaDonService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public HoaDonService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<HoaDonVm>> GetAllAsync()
        {
            var entities = await _context.HoaDons
                .OrderByDescending(x => x.TGVao)
                .ToListAsync();
            return _mapper.Map<List<HoaDonVm>>(entities);
        }

        public async Task<HoaDonVm> GetByIdAsync(string soHd)
        {
            var entity = await _context.HoaDons
                .Include(x => x.CTHDs)
                .FirstOrDefaultAsync(x => x.SoHD == soHd);

            if (entity == null) return null;
            return _mapper.Map<HoaDonVm>(entity);
        }

        public async Task<HoaDonVm> CreateAsync(HoaDonCreateRequest request)
        {
            var hd = _mapper.Map<HoaDon>(request);

            hd.SoHD = "HD" + DateTime.Now.ToString("HHmmss");
            hd.TGVao = DateTime.Now;
            hd.TrangThaiHD = "Chưa thanh toán";
            decimal tongTienSanPham = 0;
            int indexCT = 1;

            foreach (var cthd in hd.CTHDs)
            {
                cthd.SoHD = hd.SoHD;
                cthd.SoCTHD = $"{hd.SoHD}_{indexCT++}";
                cthd.ThanhTien = (cthd.SLSP * cthd.DonGia) - cthd.GiamGia;

                tongTienSanPham += cthd.ThanhTien ?? 0;
            }
            hd.TongTien = tongTienSanPham + hd.PhuThu + hd.ThueVAT - hd.GiamGiaHD;

            _context.HoaDons.Add(hd);
            await _context.SaveChangesAsync();

            return _mapper.Map<HoaDonVm>(hd);
        }

        public async Task<bool> UpdateAsync(string soHd, HoaDonUpdateRequest request)
        {
            var hd = await _context.HoaDons.FindAsync(soHd);
            if (hd == null) return false;

            _mapper.Map(request, hd);

            _context.HoaDons.Update(hd);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(string soHd)
        {
            var hd = await _context.HoaDons
                .Include(x => x.CTHDs)
                .FirstOrDefaultAsync(x => x.SoHD == soHd);

            if (hd == null) return false;

            // Xóa chi tiết hóa đơn (Nếu DB không có cấu hình Cascade Delete)
            if (hd.CTHDs != null && hd.CTHDs.Any())
            {
                _context.CTHDs.RemoveRange(hd.CTHDs);
            }

            _context.HoaDons.Remove(hd);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}