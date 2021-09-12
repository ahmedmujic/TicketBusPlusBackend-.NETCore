using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookingManagement.Migrations
{
    public partial class reducedNumberOfDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatesRoutes");

            migrationBuilder.DropTable(
                name: "Dates");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Routes");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndingDate",
                table: "Routes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartingDate",
                table: "Routes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndingDate",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "StartingDate",
                table: "Routes");

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Dates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatesRoutes",
                columns: table => new
                {
                    DatesId = table.Column<int>(type: "int", nullable: false),
                    RoutesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatesRoutes", x => new { x.DatesId, x.RoutesId });
                    table.ForeignKey(
                        name: "FK_DatesRoutes_Dates_DatesId",
                        column: x => x.DatesId,
                        principalTable: "Dates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DatesRoutes_Routes_RoutesId",
                        column: x => x.RoutesId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatesRoutes_RoutesId",
                table: "DatesRoutes",
                column: "RoutesId");
        }
    }
}
