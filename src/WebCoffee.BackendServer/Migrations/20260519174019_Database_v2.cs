using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebCoffee.BackendServer.Migrations
{
    /// <inheritdoc />
    public partial class Database_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CONGTHUC");

            migrationBuilder.DropTable(
                name: "CTPHIEUNHAP_NL");

            migrationBuilder.DropTable(
                name: "CTPNHAP");

            migrationBuilder.DropTable(
                name: "LICHSU_KHO");

            migrationBuilder.DropTable(
                name: "PHIEUNHAP_NL");

            migrationBuilder.DropTable(
                name: "PNHAP");

            migrationBuilder.DropTable(
                name: "NGUYENLIEU");

            migrationBuilder.DropTable(
                name: "NCC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NCC",
                columns: table => new
                {
                    MaNCC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DiaChiNCC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GhiChuNCC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SDTNCC = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    SoTKNCC = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TenNCC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
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
                    DVTNL = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DonGiaNL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GhiChuNL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    HanSuDung = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MucCanhBao = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SoLuongTon = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TenNL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrangThaiNL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NGUYENLIEU", x => x.MaNL);
                });

            migrationBuilder.CreateTable(
                name: "PHIEUNHAP_NL",
                columns: table => new
                {
                    SoPNNL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaNCC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaNV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GhiChuPNNL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NgayNhapNL = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TongTienNhapNL = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
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
                    GhiChuPN = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NhapHang = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TongTienNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
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
                name: "CONGTHUC",
                columns: table => new
                {
                    MaCT = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaNL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GhiChuCT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SoLuongNL = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
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
                name: "LICHSU_KHO",
                columns: table => new
                {
                    MaLSKho = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaNL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NguoiThucHien = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GhiChuLSK = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LoaiGD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SoLuongGD = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ThoiGianGD = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "CTPHIEUNHAP_NL",
                columns: table => new
                {
                    SoCTPNNL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaNL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SoPNNL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DonGiaNhapNL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SLNhapNL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    MaSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SoPN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DonGiaNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GhiChuCTPN = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SLN = table.Column<int>(type: "int", nullable: true),
                    ThanhTienNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
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
                table: "NGUYENLIEU",
                columns: new[] { "MaNL", "DVTNL", "DonGiaNL", "GhiChuNL", "HanSuDung", "MucCanhBao", "SoLuongTon", "TenNL", "TrangThaiNL" },
                values: new object[,]
                {
                    { "NL01", "Kg", 150000m, null, null, null, 10m, "Hạt Cà Phê Robusta", null },
                    { "NL02", "Lon", 25000m, null, null, null, 20m, "Sữa Đặc", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CONGTHUC_MaNL",
                table: "CONGTHUC",
                column: "MaNL");

            migrationBuilder.CreateIndex(
                name: "IX_CONGTHUC_MaSP",
                table: "CONGTHUC",
                column: "MaSP");

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
                name: "IX_LICHSU_KHO_MaNL",
                table: "LICHSU_KHO",
                column: "MaNL");

            migrationBuilder.CreateIndex(
                name: "IX_LICHSU_KHO_NguoiThucHien",
                table: "LICHSU_KHO",
                column: "NguoiThucHien");

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
        }
    }
}
