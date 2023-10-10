using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DockingAdminPanel.Migrations
{
    /// <inheritdoc />
    public partial class _31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "fee",
                table: "doctors",
                newName: "TotalFee");

            migrationBuilder.RenameColumn(
                name: "SecondaryFee",
                table: "doctors",
                newName: "DoctorFee");

            migrationBuilder.RenameColumn(
                name: "LabFee",
                table: "doctors",
                newName: "ClinicCharges");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalFee",
                table: "doctors",
                newName: "fee");

            migrationBuilder.RenameColumn(
                name: "DoctorFee",
                table: "doctors",
                newName: "SecondaryFee");

            migrationBuilder.RenameColumn(
                name: "ClinicCharges",
                table: "doctors",
                newName: "LabFee");
        }
    }
}
