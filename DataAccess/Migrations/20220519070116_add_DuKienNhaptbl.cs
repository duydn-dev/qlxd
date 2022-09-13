using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class add_DuKienNhaptbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DuKienNhaps",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDoiTuong = table.Column<int>(type: "int", nullable: true),
                    ChungLoai = table.Column<int>(type: "int", nullable: true),
                    SoLuong = table.Column<double>(type: "float", nullable: true),
                    NhapKhau = table.Column<double>(type: "float", nullable: true),
                    NhapNhaMayTrongNuoc = table.Column<double>(type: "float", nullable: true),
                    MuaTuTNDMKhac = table.Column<double>(type: "float", nullable: true),
                    TuSXPC = table.Column<double>(type: "float", nullable: true),
                    NhapKhac = table.Column<double>(type: "float", nullable: true),
                    NgayBC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuKienNhaps", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DuKienNhaps");
        }
    }
}
