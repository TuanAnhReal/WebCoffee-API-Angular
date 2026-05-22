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
                .ToListAsync(); // Nếu muốn lấy luôn CTHD ở list thì thêm .Include(x => x.CTHDs)

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
            // 1. Ánh xạ toàn bộ Request (bao gồm cả mảng ChiTiet) sang Entity
            var hd = _mapper.Map<HoaDon>(request);

            // 2. Cài đặt các thông tin tự động của Hóa Đơn
            hd.SoHD = "HD" + DateTime.Now.ToString("yyMMddHHmmss");
            hd.TGVao = DateTime.Now;
            hd.TrangThaiHD = "Chưa thanh toán";

            // 3. Xử lý Business Logic: Tính toán trên các Chi Tiết Hóa Đơn (đã được map)
            decimal tongTienSanPham = 0;
            int indexCT = 1;

            foreach (var cthd in hd.CTHDs)
            {
                cthd.SoHD = hd.SoHD;
                cthd.SoCTHD = $"{hd.SoHD}_{indexCT++}";
                cthd.ThanhTien = (cthd.SLSP * cthd.DonGia) - cthd.GiamGia;

                tongTienSanPham += cthd.ThanhTien ?? 0;
            }

            // 4. Chốt Tổng Tiền
            hd.TongTien = tongTienSanPham + hd.PhuThu + hd.ThueVAT - hd.GiamGiaHD;

            // 5. Lưu vào DB (EF Core sẽ tự động lưu cả Hóa Đơn và List CTHD cùng lúc)
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