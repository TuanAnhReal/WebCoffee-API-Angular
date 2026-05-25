using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data.Entities;
using System;

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

            modelBuilder.Entity<SanPham>()
                .Property(s => s.GiaVon)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<CTHD>()
                .Property(c => c.GiaVon)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<SanPham_KhuyenMai>()
                .HasKey(sk => new { sk.MaSP, sk.MaKM });

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

            // SEED DATA
            modelBuilder.Entity<PhanQuyen>().HasData(
                new PhanQuyen { MaPQ = "PQ01", TenPQ = "Quản trị viên", MoTaPQ = "Toàn quyền hệ thống" },
                new PhanQuyen { MaPQ = "PQ02", TenPQ = "Nhân viên", MoTaPQ = "Quyền bán hàng cơ bản" }
            );

            modelBuilder.Entity<LoaiNV>().HasData(
                new LoaiNV { MaLoaiNV = "LNV01", TenLoaiNV = "Quản lý", HsLoaiNV = 2.0 },
                new LoaiNV { MaLoaiNV = "LNV02", TenLoaiNV = "Pha chế", HsLoaiNV = 1.2 },
                new LoaiNV { MaLoaiNV = "LNV03", TenLoaiNV = "Phục vụ", HsLoaiNV = 1.0 }
            );

            modelBuilder.Entity<NhanVien>().HasData(
                new NhanVien { MaNV = "NV01", MaLoaiNV = "LNV01", HoNV = "Nguyễn Văn", TenNV = "An", PhaiNV = true, TrangThaiNV = "Đang làm" },
                new NhanVien { MaNV = "NV02", MaLoaiNV = "LNV02", HoNV = "Trần Thị", TenNV = "Bình", PhaiNV = false, TrangThaiNV = "Đang làm" }
            );

            modelBuilder.Entity<TaiKhoan>().HasData(
                new TaiKhoan { TenDangNhap = "admin", MaNV = "NV01", MaPQ = "PQ01", MatKhau = "$2a$12$Ni4r2Ts3e0aM8IS4GEk3/uS.TlZ2jSVhgiZOuKAR0XyzfjDL0Ts.y", TrangThaiTK = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "nhanvien1", MaNV = "NV02", MaPQ = "PQ02", MatKhau = "$2a$12$Ni4r2Ts3e0aM8IS4GEk3/uS.TlZ2jSVhgiZOuKAR0XyzfjDL0Ts.y", TrangThaiTK = "Hoạt động" }
            );

            modelBuilder.Entity<LoaiSP>().HasData(
                new LoaiSP { MaLoaiSP = "LSP01", TenLoaiSP = "Cà phê", LaHangDeVo = false },
                new LoaiSP { MaLoaiSP = "LSP02", TenLoaiSP = "Trà trái cây", LaHangDeVo = false },
                new LoaiSP { MaLoaiSP = "LSP03", TenLoaiSP = "Bánh ngọt", LaHangDeVo = true }
            );

            modelBuilder.Entity<SanPham>().HasData(
                new SanPham { MaSP = "SP01", MaLoaiSP = "LSP01", TenSP = "Cà phê đen đá", GiaVon = 8000m, DonGia = 25000m, DVT = "Ly", TrangThaiSP = "Đang bán" },
                new SanPham { MaSP = "SP02", MaLoaiSP = "LSP01", TenSP = "Bạc xỉu", GiaVon = 12000m, DonGia = 30000m, DVT = "Ly", TrangThaiSP = "Đang bán" },
                new SanPham { MaSP = "SP03", MaLoaiSP = "LSP02", TenSP = "Trà đào cam sả", GiaVon = 15000m, DonGia = 45000m, DVT = "Ly", TrangThaiSP = "Đang bán" }
            );

            modelBuilder.Entity<KhachHang>().HasData(
                new KhachHang { MaKH = "KH01", TenKH = "Khách Lẻ", SDTKH = "0000000000", DiemTichLuy = 0, NgayTao = new DateTime(2024, 1, 1, 0, 0, 0) },
                new KhachHang { MaKH = "KH02", TenKH = "Nguyễn Anh Tú", SDTKH = "0901234567", DiemTichLuy = 150, NgayTao = new DateTime(2024, 5, 23, 8, 0, 0) } // Fix ngày tĩnh
            );

            modelBuilder.Entity<KhuVuc>().HasData(
                new KhuVuc { SoKV = "KV01", TenKV = "Tầng trệt (Máy lạnh)", PhuThuKV = 0m, TrangThaiKhu = "Mở cửa" },
                new KhuVuc { SoKV = "KV02", TenKV = "Sân thượng (Hút thuốc)", PhuThuKV = 5000m, TrangThaiKhu = "Mở cửa" }
            );

            modelBuilder.Entity<Ban>().HasData(
                new Ban { SoBan = "B01", SoKV = "KV01", TenBan = "Bàn 1 Trệt", TrangThaiBan = "Trống" },
                new Ban { SoBan = "B02", SoKV = "KV01", TenBan = "Bàn 2 Trệt", TrangThaiBan = "Trống" },
                new Ban { SoBan = "B03", SoKV = "KV02", TenBan = "Bàn 1 Sân Thượng", TrangThaiBan = "Trống" }
            );
        }
    }
}