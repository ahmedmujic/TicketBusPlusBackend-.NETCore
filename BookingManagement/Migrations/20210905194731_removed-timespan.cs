using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookingManagement.Migrations
{
    public partial class removedtimespan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatesRoutes_Dates_MyPropertyId",
                table: "DatesRoutes");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Routes");

            migrationBuilder.RenameColumn(
                name: "MyPropertyId",
                table: "DatesRoutes",
                newName: "DatesId");

            migrationBuilder.AddForeignKey(
                name: "FK_DatesRoutes_Dates_DatesId",
                table: "DatesRoutes",
                column: "DatesId",
                principalTable: "Dates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatesRoutes_Dates_DatesId",
                table: "DatesRoutes");

            migrationBuilder.RenameColumn(
                name: "DatesId",
                table: "DatesRoutes",
                newName: "MyPropertyId");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Routes",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddForeignKey(
                name: "FK_DatesRoutes_Dates_MyPropertyId",
                table: "DatesRoutes",
                column: "MyPropertyId",
                principalTable: "Dates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
