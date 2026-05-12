using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebCoffee.BackendServer.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KH",
                columns: table => new
                {
                    MaKH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenKH = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SDTKH = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    DiemTichLuy = table.Column<int>(type: "int", nullable: true),
                    GhiChuKH = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KH", x => x.MaKH);
                });

            migrationBuilder.CreateTable(
                name: "KHUYENMAI",
                columns: table => new
                {
                    MaKM = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenKM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LoaiKM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GiaTriKM = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NgayBD = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayKT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DieuKienKM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrangThaiKM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChuKM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHUYENMAI", x => x.MaKM);
                });

            migrationBuilder.CreateTable(
                name: "KV",
                columns: table => new
                {
                    SoKV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenKV = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TGMo = table.Column<TimeSpan>(type: "time", nullable: true),
                    TGDong = table.Column<TimeSpan>(type: "time", nullable: true),
                    TrangThaiKhu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhuThuKV = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GhiChuKV = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KV", x => x.SoKV);
                });

            migrationBuilder.CreateTable(
                name: "LOAINV",
                columns: table => new
                {
                    MaLoaiNV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenLoaiNV = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HsLoaiNV = table.Column<double>(type: "float", nullable: true),
                    CVLoaiNV = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GhiChuLoaiNV = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAINV", x => x.MaLoaiNV);
                });

            migrationBuilder.CreateTable(
                name: "LOAISP",
                columns: table => new
                {
                    MaLoaiSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenLoaiSP = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LaHangDeVo = table.Column<bool>(type: "bit", nullable: true),
                    BaoQuan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GhiChuLoaiSP = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAISP", x => x.MaLoaiSP);
                });

            migrationBuilder.CreateTable(
                name: "NCC",
                columns: table => new
                {
                    MaNCC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenNCC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DiaChiNCC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SoTKNCC = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    SDTNCC = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    GhiChuNCC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NCC", x => x.MaNCC);
                });

            migrationBuilder.CreateTable(
                name: "NGUYENLIEU",
                columns: table => new
                {
                    MaNL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenNL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DVTNL = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SoLuongTon = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MucCanhBao = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DonGiaNL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HanSuDung = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThaiNL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChuNL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NGUYENLIEU", x => x.MaNL);
                });

            migrationBuilder.CreateTable(
                name: "PHANQUYEN",
                columns: table => new
                {
                    MaPQ = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenPQ = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTaPQ = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHANQUYEN", x => x.MaPQ);
                });

            migrationBuilder.CreateTable(
                name: "BAN",
                columns: table => new
                {
                    SoBan = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoKV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TenBan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrangThaiBan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChuBAN = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BAN", x => x.SoBan);
                    table.ForeignKey(
                        name: "FK_BAN_KV_SoKV",
                        column: x => x.SoKV,
                        principalTable: "KV",
                        principalColumn: "SoKV");
                });

            migrationBuilder.CreateTable(
                name: "NHANVIEN",
                columns: table => new
                {
                    MaNV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaLoaiNV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HoNV = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TenNV = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhaiNV = table.Column<bool>(type: "bit", nullable: true),
                    NgaySinhNV = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SoDTNV = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    DiaChiNV_TT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DiaChiNV_NT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SoCCCD = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SoTKNV = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TenNgHNV = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SoBHYT = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    SoBHXH = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    HinhAnhNV = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrinhDoHV = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChuNV = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrangThaiNV = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHANVIEN", x => x.MaNV);
                    table.ForeignKey(
                        name: "FK_NHANVIEN_LOAINV_MaLoaiNV",
                        column: x => x.MaLoaiNV,
                        principalTable: "LOAINV",
                        principalColumn: "MaLoaiNV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SANPHAM",
                columns: table => new
                {
                    MaSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaLoaiSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenSP = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DVT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    KichThuoc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrangThaiSP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SANPHAM", x => x.MaSP);
                    table.ForeignKey(
                        name: "FK_SANPHAM_LOAISP_MaLoaiSP",
                        column: x => x.MaLoaiSP,
                        principalTable: "LOAISP",
                        principalColumn: "MaLoaiSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DATBAN",
                columns: table => new
                {
                    MaDatBan = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaKH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SoBan = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NgayDat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GioDat = table.Column<TimeSpan>(type: "time", nullable: true),
                    SoLuongNguoi = table.Column<int>(type: "int", nullable: true),
                    TrangThaiDat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChuDat = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DATBAN", x => x.MaDatBan);
                    table.ForeignKey(
                        name: "FK_DATBAN_BAN_SoBan",
                        column: x => x.SoBan,
                        principalTable: "BAN",
                        principalColumn: "SoBan");
                    table.ForeignKey(
                        name: "FK_DATBAN_KH_MaKH",
                        column: x => x.MaKH,
                        principalTable: "KH",
                        principalColumn: "MaKH");
                });

            migrationBuilder.CreateTable(
                name: "HOADON",
                columns: table => new
                {
                    SoHD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaKH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SoBan = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaNV_PV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaNV_PC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TGVao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TGRa = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GiamGiaHD = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhuThu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ThueVAT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TrangThaiHD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HoaDonKyNhan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GhiChuHD = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOADON", x => x.SoHD);
                    table.ForeignKey(
                        name: "FK_HOADON_BAN_SoBan",
                        column: x => x.SoBan,
                        principalTable: "BAN",
                        principalColumn: "SoBan");
                    table.ForeignKey(
                        name: "FK_HOADON_KH_MaKH",
                        column: x => x.MaKH,
                        principalTable: "KH",
                        principalColumn: "MaKH");
                    table.ForeignKey(
                        name: "FK_HOADON_NHANVIEN_MaNV_PC",
                        column: x => x.MaNV_PC,
                        principalTable: "NHANVIEN",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HOADON_NHANVIEN_MaNV_PV",
                        column: x => x.MaNV_PV,
                        principalTable: "NHANVIEN",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LICHSU_KHO",
                columns: table => new
                {
                    MaLSKho = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaNL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NguoiThucHien = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LoaiGD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SoLuongGD = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ThoiGianGD = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GhiChuLSK = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LICHSU_KHO", x => x.MaLSKho);
                    table.ForeignKey(
                        name: "FK_LICHSU_KHO_NGUYENLIEU_MaNL",
                        column: x => x.MaNL,
                        principalTable: "NGUYENLIEU",
                        principalColumn: "MaNL");
                    table.ForeignKey(
                        name: "FK_LICHSU_KHO_NHANVIEN_NguoiThucHien",
                        column: x => x.NguoiThucHien,
                        principalTable: "NHANVIEN",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "PHIEUNHAP_NL",
                columns: table => new
                {
                    SoPNNL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaNCC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaNV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NgayNhapNL = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TongTienNhapNL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GhiChuPNNL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHIEUNHAP_NL", x => x.SoPNNL);
                    table.ForeignKey(
                        name: "FK_PHIEUNHAP_NL_NCC_MaNCC",
                        column: x => x.MaNCC,
                        principalTable: "NCC",
                        principalColumn: "MaNCC");
                    table.ForeignKey(
                        name: "FK_PHIEUNHAP_NL_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "NHANVIEN",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "PNHAP",
                columns: table => new
                {
                    SoPN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaNCC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaNV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TongTienNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NhapHang = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GhiChuPN = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PNHAP", x => x.SoPN);
                    table.ForeignKey(
                        name: "FK_PNHAP_NCC_MaNCC",
                        column: x => x.MaNCC,
                        principalTable: "NCC",
                        principalColumn: "MaNCC");
                    table.ForeignKey(
                        name: "FK_PNHAP_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "NHANVIEN",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "TAIKHOAN",
                columns: table => new
                {
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaNV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaPQ = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TrangThaiTK = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LanDangNhapCuoi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAIKHOAN", x => x.TenDangNhap);
                    table.ForeignKey(
                        name: "FK_TAIKHOAN_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "NHANVIEN",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TAIKHOAN_PHANQUYEN_MaPQ",
                        column: x => x.MaPQ,
                        principalTable: "PHANQUYEN",
                        principalColumn: "MaPQ",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CONGTHUC",
                columns: table => new
                {
                    MaCT = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaNL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SoLuongNL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GhiChuCT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONGTHUC", x => x.MaCT);
                    table.ForeignKey(
                        name: "FK_CONGTHUC_NGUYENLIEU_MaNL",
                        column: x => x.MaNL,
                        principalTable: "NGUYENLIEU",
                        principalColumn: "MaNL");
                    table.ForeignKey(
                        name: "FK_CONGTHUC_SANPHAM_MaSP",
                        column: x => x.MaSP,
                        principalTable: "SANPHAM",
                        principalColumn: "MaSP");
                });

            migrationBuilder.CreateTable(
                name: "SANPHAM_KHUYENMAI",
                columns: table => new
                {
                    MaSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaKM = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SANPHAM_KHUYENMAI", x => new { x.MaSP, x.MaKM });
                    table.ForeignKey(
                        name: "FK_SANPHAM_KHUYENMAI_KHUYENMAI_MaKM",
                        column: x => x.MaKM,
                        principalTable: "KHUYENMAI",
                        principalColumn: "MaKM",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SANPHAM_KHUYENMAI_SANPHAM_MaSP",
                        column: x => x.MaSP,
                        principalTable: "SANPHAM",
                        principalColumn: "MaSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CTHD",
                columns: table => new
                {
                    SoCTHD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoHD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SLSP = table.Column<int>(type: "int", nullable: true),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GiamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTHD", x => x.SoCTHD);
                    table.ForeignKey(
                        name: "FK_CTHD_HOADON_SoHD",
                        column: x => x.SoHD,
                        principalTable: "HOADON",
                        principalColumn: "SoHD");
                    table.ForeignKey(
                        name: "FK_CTHD_SANPHAM_MaSP",
                        column: x => x.MaSP,
                        principalTable: "SANPHAM",
                        principalColumn: "MaSP");
                });

            migrationBuilder.CreateTable(
                name: "THANHTOAN",
                columns: table => new
                {
                    MaTT = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoHD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PhuongThucTT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SoTienTT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ThoiGianTT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThaiTT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THANHTOAN", x => x.MaTT);
                    table.ForeignKey(
                        name: "FK_THANHTOAN_HOADON_SoHD",
                        column: x => x.SoHD,
                        principalTable: "HOADON",
                        principalColumn: "SoHD");
                });

            migrationBuilder.CreateTable(
                name: "CTPHIEUNHAP_NL",
                columns: table => new
                {
                    SoCTPNNL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoPNNL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaNL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SLNhapNL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DonGiaNhapNL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ThanhTienNhapNL = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTPHIEUNHAP_NL", x => x.SoCTPNNL);
                    table.ForeignKey(
                        name: "FK_CTPHIEUNHAP_NL_NGUYENLIEU_MaNL",
                        column: x => x.MaNL,
                        principalTable: "NGUYENLIEU",
                        principalColumn: "MaNL");
                    table.ForeignKey(
                        name: "FK_CTPHIEUNHAP_NL_PHIEUNHAP_NL_SoPNNL",
                        column: x => x.SoPNNL,
                        principalTable: "PHIEUNHAP_NL",
                        principalColumn: "SoPNNL");
                });

            migrationBuilder.CreateTable(
                name: "CTPNHAP",
                columns: table => new
                {
                    SoCTPN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoPN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SLN = table.Column<int>(type: "int", nullable: true),
                    DonGiaNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ThanhTienNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GhiChuCTPN = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTPNHAP", x => x.SoCTPN);
                    table.ForeignKey(
                        name: "FK_CTPNHAP_PNHAP_SoPN",
                        column: x => x.SoPN,
                        principalTable: "PNHAP",
                        principalColumn: "SoPN");
                    table.ForeignKey(
                        name: "FK_CTPNHAP_SANPHAM_MaSP",
                        column: x => x.MaSP,
                        principalTable: "SANPHAM",
                        principalColumn: "MaSP");
                });

            migrationBuilder.InsertData(
                table: "KV",
                columns: new[] { "SoKV", "GhiChuKV", "PhuThuKV", "TGDong", "TGMo", "TenKV", "TrangThaiKhu" },
                values: new object[,]
                {
                    { "KV01", null, 0m, null, null, "Tầng trệt (Máy lạnh)", "Mở cửa" },
                    { "KV02", null, 5000m, null, null, "Sân thượng (Hút thuốc)", "Mở cửa" }
                });

            migrationBuilder.InsertData(
                table: "LOAINV",
                columns: new[] { "MaLoaiNV", "CVLoaiNV", "GhiChuLoaiNV", "HsLoaiNV", "TenLoaiNV" },
                values: new object[,]
                {
                    { "LNV01", null, null, 2.0, "Quản lý" },
                    { "LNV02", null, null, 1.2, "Pha chế" },
                    { "LNV03", null, null, 1.0, "Phục vụ" }
                });

            migrationBuilder.InsertData(
                table: "LOAISP",
                columns: new[] { "MaLoaiSP", "BaoQuan", "GhiChuLoaiSP", "LaHangDeVo", "TenLoaiSP" },
                values: new object[,]
                {
                    { "LSP01", null, null, false, "Cà phê" },
                    { "LSP02", null, null, false, "Trà trái cây" },
                    { "LSP03", null, null, true, "Bánh ngọt" }
                });

            migrationBuilder.InsertData(
                table: "NGUYENLIEU",
                columns: new[] { "MaNL", "DVTNL", "DonGiaNL", "GhiChuNL", "HanSuDung", "MucCanhBao", "SoLuongTon", "TenNL", "TrangThaiNL" },
                values: new object[,]
                {
                    { "NL01", "Kg", 150000m, null, null, null, 10m, "Hạt Cà Phê Robusta", null },
                    { "NL02", "Lon", 25000m, null, null, null, 20m, "Sữa Đặc", null }
                });

            migrationBuilder.InsertData(
                table: "PHANQUYEN",
                columns: new[] { "MaPQ", "MoTaPQ", "TenPQ" },
                values: new object[,]
                {
                    { "PQ01", "Toàn quyền hệ thống", "Quản trị viên" },
                    { "PQ02", "Quyền bán hàng cơ bản", "Nhân viên" }
                });

            migrationBuilder.InsertData(
                table: "BAN",
                columns: new[] { "SoBan", "GhiChuBAN", "SoKV", "TenBan", "TrangThaiBan" },
                values: new object[,]
                {
                    { "B01", null, "KV01", "Bàn 1 Trệt", "Trống" },
                    { "B02", null, "KV01", "Bàn 2 Trệt", "Trống" },
                    { "B03", null, "KV02", "Bàn 1 Sân Thượng", "Trống" }
                });

            migrationBuilder.InsertData(
                table: "NHANVIEN",
                columns: new[] { "MaNV", "DiaChiNV_NT", "DiaChiNV_TT", "GhiChuNV", "HinhAnhNV", "HoNV", "MaLoaiNV", "NgaySinhNV", "PhaiNV", "SoBHXH", "SoBHYT", "SoCCCD", "SoDTNV", "SoTKNV", "TenNV", "TenNgHNV", "TrangThaiNV", "TrinhDoHV" },
                values: new object[,]
                {
                    { "NV01", null, null, null, null, "Nguyễn Văn", "LNV01", null, true, null, null, null, null, null, "An", null, "Đang làm", null },
                    { "NV02", null, null, null, null, "Trần Thị", "LNV02", null, false, null, null, null, null, null, "Bình", null, "Đang làm", null }
                });

            migrationBuilder.InsertData(
                table: "SANPHAM",
                columns: new[] { "MaSP", "DVT", "DonGia", "HinhAnh", "KichThuoc", "MaLoaiSP", "MoTa", "TenSP", "TrangThaiSP" },
                values: new object[,]
                {
                    { "SP01", "Ly", 25000m, null, null, "LSP01", null, "Cà phê đen đá", "Đang bán" },
                    { "SP02", "Ly", 30000m, null, null, "LSP01", null, "Bạc xỉu", "Đang bán" },
                    { "SP03", "Ly", 45000m, null, null, "LSP02", null, "Trà đào cam sả", "Đang bán" }
                });

            migrationBuilder.InsertData(
                table: "TAIKHOAN",
                columns: new[] { "TenDangNhap", "LanDangNhapCuoi", "MaNV", "MaPQ", "MatKhau", "TrangThaiTK" },
                values: new object[,]
                {
                    { "admin", null, "NV01", "PQ01", "123456", "Hoạt động" },
                    { "nhanvien1", null, "NV02", "PQ02", "123456", "Hoạt động" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BAN_SoKV",
                table: "BAN",
                column: "SoKV");

            migrationBuilder.CreateIndex(
                name: "IX_CONGTHUC_MaNL",
                table: "CONGTHUC",
                column: "MaNL");

            migrationBuilder.CreateIndex(
                name: "IX_CONGTHUC_MaSP",
                table: "CONGTHUC",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_CTHD_MaSP",
                table: "CTHD",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_CTHD_SoHD",
                table: "CTHD",
                column: "SoHD");

            migrationBuilder.CreateIndex(
                name: "IX_CTPHIEUNHAP_NL_MaNL",
                table: "CTPHIEUNHAP_NL",
                column: "MaNL");

            migrationBuilder.CreateIndex(
                name: "IX_CTPHIEUNHAP_NL_SoPNNL",
                table: "CTPHIEUNHAP_NL",
                column: "SoPNNL");

            migrationBuilder.CreateIndex(
                name: "IX_CTPNHAP_MaSP",
                table: "CTPNHAP",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_CTPNHAP_SoPN",
                table: "CTPNHAP",
                column: "SoPN");

            migrationBuilder.CreateIndex(
                name: "IX_DATBAN_MaKH",
                table: "DATBAN",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_DATBAN_SoBan",
                table: "DATBAN",
                column: "SoBan");

            migrationBuilder.CreateIndex(
                name: "IX_HOADON_MaKH",
                table: "HOADON",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_HOADON_MaNV_PC",
                table: "HOADON",
                column: "MaNV_PC");

            migrationBuilder.CreateIndex(
                name: "IX_HOADON_MaNV_PV",
                table: "HOADON",
                column: "MaNV_PV");

            migrationBuilder.CreateIndex(
                name: "IX_HOADON_SoBan",
                table: "HOADON",
                column: "SoBan");

            migrationBuilder.CreateIndex(
                name: "IX_LICHSU_KHO_MaNL",
                table: "LICHSU_KHO",
                column: "MaNL");

            migrationBuilder.CreateIndex(
                name: "IX_LICHSU_KHO_NguoiThucHien",
                table: "LICHSU_KHO",
                column: "NguoiThucHien");

            migrationBuilder.CreateIndex(
                name: "IX_NHANVIEN_MaLoaiNV",
                table: "NHANVIEN",
                column: "MaLoaiNV");

            migrationBuilder.CreateIndex(
                name: "IX_PHIEUNHAP_NL_MaNCC",
                table: "PHIEUNHAP_NL",
                column: "MaNCC");

            migrationBuilder.CreateIndex(
                name: "IX_PHIEUNHAP_NL_MaNV",
                table: "PHIEUNHAP_NL",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_PNHAP_MaNCC",
                table: "PNHAP",
                column: "MaNCC");

            migrationBuilder.CreateIndex(
                name: "IX_PNHAP_MaNV",
                table: "PNHAP",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_SANPHAM_MaLoaiSP",
                table: "SANPHAM",
                column: "MaLoaiSP");

            migrationBuilder.CreateIndex(
                name: "IX_SANPHAM_KHUYENMAI_MaKM",
                table: "SANPHAM_KHUYENMAI",
                column: "MaKM");

            migrationBuilder.CreateIndex(
                name: "IX_TAIKHOAN_MaNV",
                table: "TAIKHOAN",
                column: "MaNV",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TAIKHOAN_MaPQ",
                table: "TAIKHOAN",
                column: "MaPQ");

            migrationBuilder.CreateIndex(
                name: "IX_THANHTOAN_SoHD",
                table: "THANHTOAN",
                column: "SoHD");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CONGTHUC");

            migrationBuilder.DropTable(
                name: "CTHD");

            migrationBuilder.DropTable(
                name: "CTPHIEUNHAP_NL");

            migrationBuilder.DropTable(
                name: "CTPNHAP");

            migrationBuilder.DropTable(
                name: "DATBAN");

            migrationBuilder.DropTable(
                name: "LICHSU_KHO");

            migrationBuilder.DropTable(
                name: "SANPHAM_KHUYENMAI");

            migrationBuilder.DropTable(
                name: "TAIKHOAN");

            migrationBuilder.DropTable(
                name: "THANHTOAN");

            migrationBuilder.DropTable(
                name: "PHIEUNHAP_NL");

            migrationBuilder.DropTable(
                name: "PNHAP");

            migrationBuilder.DropTable(
                name: "NGUYENLIEU");

            migrationBuilder.DropTable(
                name: "KHUYENMAI");

            migrationBuilder.DropTable(
                name: "SANPHAM");

            migrationBuilder.DropTable(
                name: "PHANQUYEN");

            migrationBuilder.DropTable(
                name: "HOADON");

            migrationBuilder.DropTable(
                name: "NCC");

            migrationBuilder.DropTable(
                name: "LOAISP");

            migrationBuilder.DropTable(
                name: "BAN");

            migrationBuilder.DropTable(
                name: "KH");

            migrationBuilder.DropTable(
                name: "NHANVIEN");

            migrationBuilder.DropTable(
                name: "KV");

            migrationBuilder.DropTable(
                name: "LOAINV");
        }
    }
}
