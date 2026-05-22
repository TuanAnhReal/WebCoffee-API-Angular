using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCoffee.BackendServer.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "REFRESHTOKEN",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REFRESHTOKEN", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REFRESHTOKEN_TAIKHOAN_TenDangNhap",
                        column: x => x.TenDangNhap,
                        principalTable: "TAIKHOAN",
                        principalColumn: "TenDangNhap",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_REFRESHTOKEN_TenDangNhap",
                table: "REFRESHTOKEN",
                column: "TenDangNhap");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "REFRESHTOKEN");
        }
    }
}
