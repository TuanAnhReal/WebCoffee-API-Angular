using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog;

namespace WebCoffee.BackendServer.Services.SanPhams
{
    public class SanPhamService : ISanPhamService
    {
        private readonly AppDbContext _context;

        public SanPhamService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SanPhamVm>> GetAllAsync()
        {
            return await _context.SanPhams
                .Select(x => new SanPhamVm()
                {
                    MaSP = x.MaSP,
                    TenSP = x.TenSP,
                    DonGia = x.DonGia,
                    HinhAnh = x.HinhAnh ?? "", // Xử lý null vì DTO không cho phép HinhAnh null
                    TrangThaiSP = x.TrangThaiSP
                }).ToListAsync();
        }

        public async Task<SanPhamVm> GetByIdAsync(string maSp)
        {
            // Tìm theo khóa chính kiểu string
            var sp = await _context.SanPhams.FindAsync(maSp);
            if (sp == null) return null;

            return new SanPhamVm()
            {
                MaSP = sp.MaSP,
                TenSP = sp.TenSP,
                DonGia = sp.DonGia,
                HinhAnh = sp.HinhAnh ?? "",
                TrangThaiSP = sp.TrangThaiSP
            };
        }

        public async Task<string> CreateAsync(SanPhamCreateRequest request)
        {
            string newMaSP = "SP" + DateTime.Now.ToString("HHmmss");

            var sp = new SanPham()
            {
                MaSP = newMaSP,
                TenSP = request.TenSP ?? "Chưa có tên",
                DonGia = request.DonGia,
                MaLoaiSP = request.MaLoaiSP ?? "MACDINH",
                TrangThaiSP = "Đang bán"
            };

            _context.SanPhams.Add(sp);
            await _context.SaveChangesAsync();

            return sp.MaSP;
        }

        public async Task<int> UpdateAsync(string maSp, SanPhamUpdateRequest request)
        {
            var sp = await _context.SanPhams.FindAsync(maSp);
            if (sp == null) return 0;

            // Cập nhật các trường. Nếu request có null thì giữ nguyên giá trị cũ
            sp.TenSP = request.TenSP ?? sp.TenSP;
            sp.DonGia = request.DonGia;
            sp.TrangThaiSP = request.TrangThaiSP ?? sp.TrangThaiSP;

            _context.SanPhams.Update(sp);
            return await _context.SaveChangesAsync(); // Trả về số lượng records bị ảnh hưởng
        }

        public async Task<int> DeleteAsync(string maSp)
        {
            var sp = await _context.SanPhams.FindAsync(maSp);
            if (sp == null) return 0;

            _context.SanPhams.Remove(sp);
            return await _context.SaveChangesAsync();
        }
    }
}