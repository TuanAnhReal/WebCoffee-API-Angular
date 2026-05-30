using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.BackendServer.Services.Storage;
using WebCoffee.ViewModels.Catalog.SanPhams;
using WebCoffee.ViewModels.Common; // Đảm bảo đã có namespace chứa LoaiKhuyenMaiConstants

namespace WebCoffee.BackendServer.Services.SanPhams
{
    public class SanPhamService : ISanPhamService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;

        public SanPhamService(
            AppDbContext context,
            IMapper mapper,
            IStorageService storageService)
        {
            _context = context;
            _mapper = mapper;
            _storageService = storageService;
        }

        public async Task<List<SanPhamVm>> GetAllAsync()
        {
            // Eager Loading đầy đủ các bảng liên kết
            var entities = await _context.SanPhams
                .Include(x => x.LoaiSP)
                .Include(x => x.SanPham_KhuyenMais)
                    .ThenInclude(spkm => spkm.KhuyenMai)
                .ToListAsync();

            var vms = _mapper.Map<List<SanPhamVm>>(entities);

            foreach (var vm in vms)
            {
                var entity = entities.First(x => x.MaSP == vm.MaSp);
                ApplyPromotion(vm, entity);
            }

            return vms;
        }

        public async Task<SanPhamVm?> GetByIdAsync(string maSp)
        {
            var entity = await _context.SanPhams
                .Include(x => x.LoaiSP)
                .Include(x => x.SanPham_KhuyenMais)
                    .ThenInclude(spkm => spkm.KhuyenMai)
                .FirstOrDefaultAsync(x => x.MaSP == maSp);

            if (entity == null)
                return null;

            var vm = _mapper.Map<SanPhamVm>(entity);
            ApplyPromotion(vm, entity);

            return vm;
        }

        public async Task<SanPhamVm> CreateAsync(SanPhamCreateRequest request)
        {
            var entity = _mapper.Map<SanPham>(request);

            entity.MaSP = $"SP{DateTime.Now:HHmmss}";

            if (request.HinhAnhFile != null)
            {
                entity.HinhAnh = await _storageService
                    .UploadImageAsync(request.HinhAnhFile, "SanPhams");
            }

            _context.SanPhams.Add(entity);

            await _context.SaveChangesAsync();

            await _context.Entry(entity)
                .Reference(x => x.LoaiSP)
                .LoadAsync();

            var vm = _mapper.Map<SanPhamVm>(entity);

            // Dù sản phẩm mới chưa có khuyến mãi, ta vẫn gọi hàm ApplyPromotion
            // để đảm bảo các thuộc tính như GiaSauKhuyenMai được gán bằng giá gốc.
            ApplyPromotion(vm, entity);

            return vm;
        }

        public async Task<bool> UpdateAsync(
            string maSp,
            SanPhamUpdateRequest request)
        {
            var entity = await _context.SanPhams
                .FirstOrDefaultAsync(x => x.MaSP == maSp);

            if (entity == null)
                return false;

            if (request.HinhAnhFile != null)
            {
                if (!string.IsNullOrEmpty(entity.HinhAnh))
                {
                    await _storageService.DeleteImageAsync(entity.HinhAnh);
                }

                entity.HinhAnh = await _storageService
                    .UploadImageAsync(request.HinhAnhFile, "SanPhams");
            }

            _mapper.Map(request, entity);

            _context.SanPhams.Update(entity);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string maSp)
        {
            var entity = await _context.SanPhams
                .FirstOrDefaultAsync(x => x.MaSP == maSp);

            if (entity == null)
                return false;

            if (!string.IsNullOrEmpty(entity.HinhAnh))
            {
                await _storageService.DeleteImageAsync(entity.HinhAnh);
            }

            _context.SanPhams.Remove(entity);

            return await _context.SaveChangesAsync() > 0;
        }

        // =========================================================
        // HELPER METHODS TÍNH TOÁN KHUYẾN MÃI
        // =========================================================

        private KhuyenMai? GetActivePromotion(SanPham sanPham)
        {
            // LOGGING TẠM THỜI: Kiểm tra số lượng liên kết load được
            Console.WriteLine($"[DEBUG] SP: {sanPham.MaSP} - Số lượng liên kết KM: {sanPham.SanPham_KhuyenMais?.Count ?? 0}");

            if (sanPham.SanPham_KhuyenMais == null || !sanPham.SanPham_KhuyenMais.Any())
                return null;

            // Đưa về mốc 00:00:00 để tránh sai số do Giờ/Phút/Giây
            var today = DateTime.Now.Date;

            foreach (var spkm in sanPham.SanPham_KhuyenMais)
            {
                var km = spkm.KhuyenMai;
                if (km == null) continue;

                bool isValidStartDate = km.NgayBD.HasValue && km.NgayBD.Value.Date <= today;
                bool isValidEndDate = km.NgayKT.HasValue && km.NgayKT.Value.Date >= today;

                // LOGGING TẠM THỜI: Xem lý do KM bị loại
                Console.WriteLine($"[DEBUG] --- KM: {km.MaKM} | Loai: {km.LoaiKM} | NgayBD: {km.NgayBD?.Date:dd/MM/yyyy} (Valid: {isValidStartDate}) | NgayKT: {km.NgayKT?.Date:dd/MM/yyyy} (Valid: {isValidEndDate}) | Today: {today:dd/MM/yyyy}");

                if (isValidStartDate && isValidEndDate)
                {
                    return km; // Trả về khuyến mãi hợp lệ đầu tiên
                }
            }

            return null;
        }

        private decimal CalculatePromotionPrice(decimal originalPrice, KhuyenMai km)
        {
            decimal giaSauKm = originalPrice;

            if (km.LoaiKM == LoaiKhuyenMaiConstants.Percent)
            {
                giaSauKm = originalPrice - (originalPrice * (km.GiaTriKM ?? 0) / 100);
            }
            else if (km.LoaiKM == LoaiKhuyenMaiConstants.Amount)
            {
                giaSauKm = originalPrice - (km.GiaTriKM ?? 0);
            }

            return giaSauKm < 0 ? 0 : giaSauKm;
        }

        private void ApplyPromotion(SanPhamVm vm, SanPham entity)
        {
            var activePromo = GetActivePromotion(entity);

            if (activePromo != null)
            {
                vm.CoKhuyenMai = true;
                vm.MaKhuyenMai = activePromo.MaKM;
                vm.TenKhuyenMai = activePromo.TenKM;
                vm.LoaiKhuyenMai = activePromo.LoaiKM;
                vm.GiaTriKhuyenMai = activePromo.GiaTriKM;
                vm.GiaSauKhuyenMai = CalculatePromotionPrice(vm.GiaSp, activePromo);
            }
            else
            {
                vm.CoKhuyenMai = false;
                vm.MaKhuyenMai = null;
                vm.TenKhuyenMai = null;
                vm.LoaiKhuyenMai = null;
                vm.GiaTriKhuyenMai = null;
                vm.GiaSauKhuyenMai = vm.GiaSp; // Giá không đổi
            }
        }
    }
}