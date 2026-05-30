using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCoffee.BackendServer.Migrations
{
    /// <inheritdoc />
    public partial class AddPromotionSnapshotToCTHD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "GiaGoc",
                table: "CTHD",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GiaTriKM",
                table: "CTHD",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoaiKM",
                table: "CTHD",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaKM",
                table: "CTHD",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenKM",
                table: "CTHD",
                type: "nvarchar(255)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiaGoc",
                table: "CTHD");

            migrationBuilder.DropColumn(
                name: "GiaTriKM",
                table: "CTHD");

            migrationBuilder.DropColumn(
                name: "LoaiKM",
                table: "CTHD");

            migrationBuilder.DropColumn(
                name: "MaKM",
                table: "CTHD");

            migrationBuilder.DropColumn(
                name: "TenKM",
                table: "CTHD");
        }
    }
}
