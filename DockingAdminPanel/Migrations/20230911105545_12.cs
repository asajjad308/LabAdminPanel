using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DockingAdminPanel.Migrations
{
    /// <inheritdoc />
    public partial class _12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "doctors");

            migrationBuilder.AddColumn<double>(
                name: "fee",
                table: "doctors",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fee",
                table: "doctors");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "doctors",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
