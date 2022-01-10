using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop.Infrastructure.Identity.Migrations
{
    public partial class ShopDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kraj",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Znacznik = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    NazwaPolska = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NazwaAngielska = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UE = table.Column<bool>(type: "bit", nullable: false),
                    KodKrajuISO2 = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    KodKrajuISO3 = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    KodWalutyISO = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kraj", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Kraj",
                columns: new[] { "Id", "KodKrajuISO2", "KodKrajuISO3", "KodWalutyISO", "NazwaAngielska", "NazwaPolska", "UE" },
                values: new object[] { 1, "PL", "POL", "PLN", "Poland", "Polska", true });

            migrationBuilder.CreateIndex(
                name: "UIX_Kraj__KodKrajuISO2",
                table: "Kraj",
                column: "KodKrajuISO2",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UIX_Kraj__KodKrajuISO3",
                table: "Kraj",
                column: "KodKrajuISO3",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kraj");
        }
    }
}
