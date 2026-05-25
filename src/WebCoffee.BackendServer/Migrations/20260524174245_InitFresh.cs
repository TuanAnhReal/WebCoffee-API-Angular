using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCoffee.BackendServer.Migrations
{
    /// <inheritdoc />
    public partial class InitFresh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TAIKHOAN",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "MatKhau",
                value: "$2a$12$Ni4r2Ts3e0aM8IS4GEk3/uS.TlZ2jSVhgiZOuKAR0XyzfjDL0Ts.y");

            migrationBuilder.UpdateData(
                table: "TAIKHOAN",
                keyColumn: "TenDangNhap",
                keyValue: "nhanvien1",
                column: "MatKhau",
                value: "$2a$12$Ni4r2Ts3e0aM8IS4GEk3/uS.TlZ2jSVhgiZOuKAR0XyzfjDL0Ts.y");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
