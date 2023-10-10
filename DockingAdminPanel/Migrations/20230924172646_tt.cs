﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DockingAdminPanel.Migrations
{
    /// <inheritdoc />
    public partial class tt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "patientsTests",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TestName",
                table: "patientsTests",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "patientsTests");

            migrationBuilder.DropColumn(
                name: "TestName",
                table: "patientsTests");
        }
    }
}
