using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebCoffee.BackendServer.Migrations
{
    /// <inheritdoc />
    public partial class Seed_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "GiaVon",
                table: "SANPHAM",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTao",
                table: "KH",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "GiaVon",
                table: "CTHD",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "KH",
                columns: new[] { "MaKH", "DiemTichLuy", "GhiChuKH", "NgayTao", "SDTKH", "TenKH" },
                values: new object[,]
                {
                    { "KH01", 0, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0000000000", "Khách Lẻ" },
                    { "KH02", 150, null, new DateTime(2024, 5, 23, 8, 0, 0, 0, DateTimeKind.Unspecified), "0901234567", "Nguyễn Anh Tú" }
                });

            migrationBuilder.UpdateData(
                table: "SANPHAM",
                keyColumn: "MaSP",
                keyValue: "SP01",
                column: "GiaVon",
                value: 8000m);

            migrationBuilder.UpdateData(
                table: "SANPHAM",
                keyColumn: "MaSP",
                keyValue: "SP02",
                column: "GiaVon",
                value: 12000m);

            migrationBuilder.UpdateData(
                table: "SANPHAM",
                keyColumn: "MaSP",
                keyValue: "SP03",
                column: "GiaVon",
                value: 15000m);

            migrationBuilder.UpdateData(
                table: "TAIKHOAN",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "MatKhau",
                value: "$2a$11$mC7M8Wq.qWqT2C9Ww9w9eOn3Zf8g6X7y8u9I0o1P2q3R4s5T6u7vW");

            migrationBuilder.UpdateData(
                table: "TAIKHOAN",
                keyColumn: "TenDangNhap",
                keyValue: "nhanvien1",
                column: "MatKhau",
                value: "$2a$11$mC7M8Wq.qWqT2C9Ww9w9eOn3Zf8g6X7y8u9I0o1P2q3R4s5T6u7vW");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KH",
                keyColumn: "MaKH",
                keyValue: "KH01");

            migrationBuilder.DeleteData(
                table: "KH",
                keyColumn: "MaKH",
                keyValue: "KH02");

            migrationBuilder.DropColumn(
                name: "GiaVon",
                table: "SANPHAM");

            migrationBuilder.DropColumn(
                name: "NgayTao",
                table: "KH");

            migrationBuilder.DropColumn(
                name: "GiaVon",
                table: "CTHD");

            migrationBuilder.UpdateData(
                table: "TAIKHOAN",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "MatKhau",
                value: "123456");

            migrationBuilder.UpdateData(
                table: "TAIKHOAN",
                keyColumn: "TenDangNhap",
                keyValue: "nhanvien1",
                column: "MatKhau",
                value: "123456");
        }
    }
}
