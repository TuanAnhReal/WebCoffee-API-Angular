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
                .Include(x => x.CTHDs)
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

            if (!string.IsNullOrEmpty(request.SoBan) && request.SoBan != "Mang đi")
            {
                var ban = await _context.Bans.FindAsync(request.SoBan);
                if (ban != null && ban.TrangThaiBan == "Trống")
                {
                    ban.TrangThaiBan = "Đang phục vụ";
                    _context.Bans.Update(ban);
                    await _context.SaveChangesAsync();
                }
            }

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

        public async Task<bool> UpdateStatusAsync(string soHd, string newStatus)
        {
            var hd = await _context.HoaDons.FindAsync(soHd);
            if (hd == null) return false;
            hd.TrangThaiHD = newStatus;

            if (newStatus == "Đã thanh toán" || newStatus == "Đã hủy")
            {
                hd.TGRa = DateTime.Now;

                if (!string.IsNullOrEmpty(hd.SoBan) && hd.SoBan != "Mang đi")
                {
                    var ban = await _context.Bans.FindAsync(hd.SoBan);
                    if (ban != null)
                    {
                        ban.TrangThaiBan = "Trống";
                        _context.Bans.Update(ban);
                    }
                }
            }

            _context.HoaDons.Update(hd);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> CompleteKitchenOrderAsync(string soHd, KitchenUpdateDto dto)
        {
            var hd = await _context.HoaDons.FindAsync(soHd);
            if (hd == null) return false;

            // Cập nhật trạng thái theo yêu cầu của bếp (thường là "Hoàn thành")
            hd.TrangThaiHD = dto.TrangThaiHD;

            // Gán mã nhân viên pha chế thực hiện đơn này
            hd.MaNV_PC = dto.MaNV_PC;

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