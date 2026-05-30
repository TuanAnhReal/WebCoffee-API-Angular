using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.HoaDons;
using WebCoffee.ViewModels.Common;

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
                    .ThenInclude(cthd => cthd.SanPham) // Quan trọng để map TenSP
                .OrderByDescending(x => x.TGVao)
                .ToListAsync();
            return _mapper.Map<List<HoaDonVm>>(entities);
        }

        public async Task<HoaDonVm> GetByIdAsync(string soHd)
        {
            var entity = await _context.HoaDons
                .Include(x => x.CTHDs)
                    .ThenInclude(cthd => cthd.SanPham) // Quan trọng để map TenSP
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

            var productIds = hd.CTHDs.Where(x => x.MaSP != null).Select(x => x.MaSP).Distinct().ToList();

            var products = await _context.SanPhams
                .Include(sp => sp.SanPham_KhuyenMais)
                    .ThenInclude(spkm => spkm.KhuyenMai)
                .Where(sp => productIds.Contains(sp.MaSP))
                .ToListAsync();

            foreach (var cthd in hd.CTHDs)
            {
                var sanPham = products.FirstOrDefault(p => p.MaSP == cthd.MaSP);

                if (sanPham != null)
                {
                    var activePromotion = GetActivePromotion(sanPham);
                    var originalPrice = sanPham.DonGia;

                    cthd.GiaGoc = originalPrice; // Lưu snapshot giá gốc
                    cthd.GiaVon = sanPham.GiaVon;

                    if (activePromotion != null)
                    {
                        var discountedPrice = CalculatePromotionPrice(originalPrice, activePromotion);

                        cthd.DonGia = discountedPrice;
                        cthd.GiamGia = (originalPrice - discountedPrice) * (cthd.SLSP ?? 0);

                        // Lưu snapshot khuyến mãi
                        cthd.MaKM = activePromotion.MaKM;
                        cthd.TenKM = activePromotion.TenKM;
                        cthd.LoaiKM = activePromotion.LoaiKM;
                        cthd.GiaTriKM = activePromotion.GiaTriKM;
                    }
                    else
                    {
                        cthd.DonGia = originalPrice;
                        cthd.GiamGia = 0;

                        // Xóa thông tin khuyến mãi nếu không có
                        cthd.MaKM = null;
                        cthd.TenKM = null;
                        cthd.LoaiKM = null;
                        cthd.GiaTriKM = null;
                    }
                }

                cthd.SoHD = hd.SoHD;
                cthd.SoCTHD = $"{hd.SoHD}_{indexCT++}";

                cthd.ThanhTien = (cthd.SLSP ?? 0) * (cthd.DonGia ?? 0);
                tongTienSanPham += cthd.ThanhTien ?? 0;
            }

            hd.TongTien = tongTienSanPham + (hd.PhuThu ?? 0) + (hd.ThueVAT ?? 0) - (hd.GiamGiaHD ?? 0);

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

        // ... Các hàm UpdateAsync, UpdateStatusAsync, CompleteKitchenOrderAsync, DeleteAsync giữ nguyên ...
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

            hd.TrangThaiHD = dto.TrangThaiHD;
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

            if (hd.CTHDs != null && hd.CTHDs.Any())
            {
                _context.CTHDs.RemoveRange(hd.CTHDs);
            }

            _context.HoaDons.Remove(hd);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        // =========================================================
        // HELPER METHODS TÍNH TOÁN KHUYẾN MÃI
        // =========================================================

        private KhuyenMai? GetActivePromotion(SanPham sanPham)
        {
            if (sanPham.SanPham_KhuyenMais == null || !sanPham.SanPham_KhuyenMais.Any())
                return null;

            var today = DateTime.Today;

            return sanPham.SanPham_KhuyenMais
                .Select(x => x.KhuyenMai)
                .FirstOrDefault(km =>
                    km != null &&
                    km.NgayBD.HasValue && km.NgayBD.Value.Date <= today &&
                    km.NgayKT.HasValue && km.NgayKT.Value.Date >= today);
        }

        private decimal CalculatePromotionPrice(decimal originalPrice, KhuyenMai km)
        {
            decimal result = originalPrice;

            if (km.LoaiKM == LoaiKhuyenMaiConstants.Percent)
            {
                result = originalPrice - (originalPrice * (km.GiaTriKM ?? 0) / 100);
            }
            else if (km.LoaiKM == LoaiKhuyenMaiConstants.Amount)
            {
                result = originalPrice - (km.GiaTriKM ?? 0);
            }

            return result < 0 ? 0 : result;
        }
    }
}