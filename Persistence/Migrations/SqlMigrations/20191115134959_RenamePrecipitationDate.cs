using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wells.Persistence.Migrations
{
    public partial class RenamePrecipitationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecipitationDate",
                table: "Precipitations");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Precipitations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Precipitations");

            migrationBuilder.AddColumn<DateTime>(
                name: "PrecipitationDate",
                table: "Precipitations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
