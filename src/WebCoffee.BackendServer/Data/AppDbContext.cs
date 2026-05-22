using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data.Entities;

namespace WebCoffee.BackendServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Khai báo các DbSet
        public DbSet<LoaiSP> LoaiSPs { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<LoaiNV> LoaiNVs { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<PhanQuyen> PhanQuyens { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<KhuVuc> KhuVucs { get; set; }
        public DbSet<Ban> Bans { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<CTHD> CTHDs { get; set; }
        public DbSet<KhuyenMai> KhuyenMais { get; set; }
        public DbSet<SanPham_KhuyenMai> SanPham_KhuyenMais { get; set; }
        public DbSet<DatBan> DatBans { get; set; }
        public DbSet<ThanhToan> ThanhToans { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình Composite Key cho bảng SANPHAM_KHUYENMAI
            modelBuilder.Entity<SanPham_KhuyenMai>()
                .HasKey(sk => new { sk.MaSP, sk.MaKM });

            // Cấu hình các quan hệ N-N, 1-N đặc thù nếu Data Annotation không đủ
            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.NhanVienPhucVu)
                .WithMany()
                .HasForeignKey(h => h.MaNV_PV)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.NhanVienPhaChe)
                .WithMany()
                .HasForeignKey(h => h.MaNV_PC)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhanQuyen>().HasData(
                new PhanQuyen { MaPQ = "PQ01", TenPQ = "Quản trị viên", MoTaPQ = "Toàn quyền hệ thống" },
                new PhanQuyen { MaPQ = "PQ02", TenPQ = "Nhân viên", MoTaPQ = "Quyền bán hàng cơ bản" }
            );

            modelBuilder.Entity<LoaiNV>().HasData(
                new LoaiNV { MaLoaiNV = "LNV01", TenLoaiNV = "Quản lý", HsLoaiNV = 2.0 },
                new LoaiNV { MaLoaiNV = "LNV02", TenLoaiNV = "Pha chế", HsLoaiNV = 1.2 },
                new LoaiNV { MaLoaiNV = "LNV03", TenLoaiNV = "Phục vụ", HsLoaiNV = 1.0 }
            );

            // --- Bảng Con: NHÂN VIÊN (Phụ thuộc LNV) ---
            modelBuilder.Entity<NhanVien>().HasData(
                new NhanVien { MaNV = "NV01", MaLoaiNV = "LNV01", HoNV = "Nguyễn Văn", TenNV = "An", PhaiNV = true, TrangThaiNV = "Đang làm" },
                new NhanVien { MaNV = "NV02", MaLoaiNV = "LNV02", HoNV = "Trần Thị", TenNV = "Bình", PhaiNV = false, TrangThaiNV = "Đang làm" }
            );

            // --- Bảng Con: TÀI KHOẢN (Phụ thuộc NV, PQ) ---
            modelBuilder.Entity<TaiKhoan>().HasData(
                // Lưu ý: Thực tế mật khẩu phải được Hash (VD: BCrypt), ở đây để chuỗi giả lập
                new TaiKhoan { TenDangNhap = "admin", MaNV = "NV01", MaPQ = "PQ01", MatKhau = "123456", TrangThaiTK = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "nhanvien1", MaNV = "NV02", MaPQ = "PQ02", MatKhau = "123456", TrangThaiTK = "Hoạt động" }
            );

            // --- Bảng Cha: LOẠI SẢN PHẨM ---
            modelBuilder.Entity<LoaiSP>().HasData(
                new LoaiSP { MaLoaiSP = "LSP01", TenLoaiSP = "Cà phê", LaHangDeVo = false },
                new LoaiSP { MaLoaiSP = "LSP02", TenLoaiSP = "Trà trái cây", LaHangDeVo = false },
                new LoaiSP { MaLoaiSP = "LSP03", TenLoaiSP = "Bánh ngọt", LaHangDeVo = true }
            );

            // --- Bảng Con: SẢN PHẨM (Phụ thuộc LSP) ---
            // Nhớ thêm chữ 'm' phía sau các số kiểu decimal
            modelBuilder.Entity<SanPham>().HasData(
                new SanPham { MaSP = "SP01", MaLoaiSP = "LSP01", TenSP = "Cà phê đen đá", DonGia = 25000m, DVT = "Ly", TrangThaiSP = "Đang bán" },
                new SanPham { MaSP = "SP02", MaLoaiSP = "LSP01", TenSP = "Bạc xỉu", DonGia = 30000m, DVT = "Ly", TrangThaiSP = "Đang bán" },
                new SanPham { MaSP = "SP03", MaLoaiSP = "LSP02", TenSP = "Trà đào cam sả", DonGia = 45000m, DVT = "Ly", TrangThaiSP = "Đang bán" }
            );

            // --- Bảng Cha: KHU VỰC ---
            modelBuilder.Entity<KhuVuc>().HasData(
                new KhuVuc { SoKV = "KV01", TenKV = "Tầng trệt (Máy lạnh)", PhuThuKV = 0m, TrangThaiKhu = "Mở cửa" },
                new KhuVuc { SoKV = "KV02", TenKV = "Sân thượng (Hút thuốc)", PhuThuKV = 5000m, TrangThaiKhu = "Mở cửa" }
            );

            // --- Bảng Con: BÀN (Phụ thuộc KV) ---
            modelBuilder.Entity<Ban>().HasData(
                new Ban { SoBan = "B01", SoKV = "KV01", TenBan = "Bàn 1 Trệt", TrangThaiBan = "Trống" },
                new Ban { SoBan = "B02", SoKV = "KV01", TenBan = "Bàn 2 Trệt", TrangThaiBan = "Trống" },
                new Ban { SoBan = "B03", SoKV = "KV02", TenBan = "Bàn 1 Sân Thượng", TrangThaiBan = "Trống" }
            );

        }
    }
}