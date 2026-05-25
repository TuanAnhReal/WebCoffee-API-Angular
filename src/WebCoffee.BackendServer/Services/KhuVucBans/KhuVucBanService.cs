using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.KhuVucBans;
using WebCoffee.ViewModels.Common;

namespace WebCoffee.BackendServer.Services.KhuVucBans
{
    public class KhuVucBanService : IKhuVucBanService
    {
        private readonly AppDbContext _context;

        public KhuVucBanService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<KhuVucVm>>> GetAllWithBansAsync()
        {
            // Lấy ngày hiện tại để chỉ check các lượt đặt bàn trong ngày
            var today = DateTime.Today;

            var data = await _context.KhuVucs
                .Include(k => k.Bans)
                    .ThenInclude(b => b.DatBans.Where(d => d.NgayDat == today && d.TrangThaiDat != "Đã hủy"))
                // Cần bổ sung public ICollection<DatBan>? DatBans { get; set; } vào Entity Ban.cs
                .Include(k => k.Bans)
                    .ThenInclude(b => b.DatBans)
                        .ThenInclude(d => d.KhachHang) // Để lấy tên khách
                .Select(k => new KhuVucVm
                {
                    SoKV = k.SoKV,
                    TenKV = k.TenKV,
                    TrangThaiKhu = k.TrangThaiKhu,
                    Bans = k.Bans.Select(b => new BanVm
                    {
                        SoBan = b.SoBan,
                        SoKV = b.SoKV,
                        TenBan = b.TenBan,
                        TrangThaiBan = b.TrangThaiBan,

                        // Lấy lượt đặt bàn gần nhất trong tương lai
                        ThoiGianDatSắpToi = b.DatBans
                            .Where(d => d.GioDat >= DateTime.Now.TimeOfDay)
                            .OrderBy(d => d.GioDat)
                            .Select(d => d.GioDat)
                            .FirstOrDefault(),

                        TenKhachDat = b.DatBans
                            .Where(d => d.GioDat >= DateTime.Now.TimeOfDay)
                            .OrderBy(d => d.GioDat)
                            .Select(d => d.KhachHang.TenKH) // Tùy thuộc vào cột tên trong Entity KhachHang của bạn
                            .FirstOrDefault()
                    }).ToList()
                })
                .ToListAsync();

            return new ApiResponse<List<KhuVucVm>> { Success = true, Data = data, StatusCode = 200 };
        }

        // ================= KHU VỰC =================
        public async Task<ApiResponse<bool>> CreateKhuVucAsync(KhuVucCreateRequest request)
        {
            if (await _context.KhuVucs.AnyAsync(x => x.SoKV == request.SoKV))
                return new ApiResponse<bool> { Success = false, Message = "Mã khu vực đã tồn tại" };

            var khuVuc = new KhuVuc
            {
                SoKV = request.SoKV,
                TenKV = request.TenKV,
                TGMo = request.TGMo,
                TGDong = request.TGDong,
                TrangThaiKhu = request.TrangThaiKhu,
                PhuThuKV = request.PhuThuKV,
                GhiChuKV = request.GhiChuKV
            };

            _context.KhuVucs.Add(khuVuc);
            await _context.SaveChangesAsync();
            return new ApiResponse<bool> { Success = true, Message = "Thêm khu vực thành công" };
        }

        public async Task<ApiResponse<bool>> UpdateKhuVucAsync(string soKV, KhuVucUpdateRequest request)
        {
            var khuVuc = await _context.KhuVucs.FindAsync(soKV);
            if (khuVuc == null) return new ApiResponse<bool> { Success = false, Message = "Không tìm thấy khu vực" };

            khuVuc.TenKV = request.TenKV;
            khuVuc.TGMo = request.TGMo;
            khuVuc.TGDong = request.TGDong;
            khuVuc.TrangThaiKhu = request.TrangThaiKhu;
            khuVuc.PhuThuKV = request.PhuThuKV;
            khuVuc.GhiChuKV = request.GhiChuKV;

            await _context.SaveChangesAsync();
            return new ApiResponse<bool> { Success = true, Message = "Cập nhật khu vực thành công" };
        }

        public async Task<ApiResponse<bool>> DeleteKhuVucAsync(string soKV)
        {
            var khuVuc = await _context.KhuVucs.Include(x => x.Bans).FirstOrDefaultAsync(x => x.SoKV == soKV);
            if (khuVuc == null) return new ApiResponse<bool> { Success = false, Message = "Không tìm thấy khu vực" };

            // Logic an toàn: Chặn xóa nếu còn bàn
            if (khuVuc.Bans != null && khuVuc.Bans.Any())
                return new ApiResponse<bool> { Success = false, Message = "Không thể xóa khu vực đang chứa bàn" };

            _context.KhuVucs.Remove(khuVuc);
            await _context.SaveChangesAsync();
            return new ApiResponse<bool> { Success = true, Message = "Xóa khu vực thành công" };
        }

        // ================= BÀN =================
        public async Task<ApiResponse<bool>> CreateBanAsync(BanCreateRequest request)
        {
            if (await _context.Bans.AnyAsync(x => x.SoBan == request.SoBan))
                return new ApiResponse<bool> { Success = false, Message = "Mã bàn đã tồn tại" };

            var ban = new Ban
            {
                SoBan = request.SoBan,
                SoKV = request.SoKV,
                TenBan = request.TenBan,
                TrangThaiBan = request.TrangThaiBan,
                GhiChuBAN = request.GhiChuBAN
            };

            _context.Bans.Add(ban);
            await _context.SaveChangesAsync();
            return new ApiResponse<bool> { Success = true, Message = "Thêm bàn thành công" };
        }

        public async Task<ApiResponse<bool>> UpdateBanAsync(string soBan, BanUpdateRequest request)
        {
            var ban = await _context.Bans.FindAsync(soBan);
            if (ban == null) return new ApiResponse<bool> { Success = false, Message = "Không tìm thấy bàn" };

            ban.SoKV = request.SoKV;
            ban.TenBan = request.TenBan;
            ban.TrangThaiBan = request.TrangThaiBan;
            ban.GhiChuBAN = request.GhiChuBAN;

            await _context.SaveChangesAsync();
            return new ApiResponse<bool> { Success = true, Message = "Cập nhật bàn thành công" };
        }

        public async Task<ApiResponse<bool>> DeleteBanAsync(string soBan)
        {
            var ban = await _context.Bans.FindAsync(soBan);
            if (ban == null) return new ApiResponse<bool> { Success = false, Message = "Không tìm thấy bàn" };

            _context.Bans.Remove(ban);
            await _context.SaveChangesAsync();
            return new ApiResponse<bool> { Success = true, Message = "Xóa bàn thành công" };
        }
    }
}