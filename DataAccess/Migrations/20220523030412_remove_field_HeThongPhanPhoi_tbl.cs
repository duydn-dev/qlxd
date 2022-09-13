using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class remove_field_HeThongPhanPhoi_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AprrovedBy",
                table: "HeThongPhanPhoi");

            migrationBuilder.DropColumn(
                name: "AprrovedDate",
                table: "HeThongPhanPhoi");

            migrationBuilder.DropColumn(
                name: "AprrovedStatus",
                table: "HeThongPhanPhoi");

            migrationBuilder.DropColumn(
                name: "MD5",
                table: "HeThongPhanPhoi");

            migrationBuilder.DropColumn(
                name: "MaPhienBan",
                table: "HeThongPhanPhoi");

            migrationBuilder.DropColumn(
                name: "RejectMessage",
                table: "HeThongPhanPhoi");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "HeThongPhanPhoi");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AprrovedBy",
                table: "HeThongPhanPhoi",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AprrovedDate",
                table: "HeThongPhanPhoi",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AprrovedStatus",
                table: "HeThongPhanPhoi",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MD5",
                table: "HeThongPhanPhoi",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaPhienBan",
                table: "HeThongPhanPhoi",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RejectMessage",
                table: "HeThongPhanPhoi",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "HeThongPhanPhoi",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
