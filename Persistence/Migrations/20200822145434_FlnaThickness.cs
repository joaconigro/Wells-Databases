using Microsoft.EntityFrameworkCore.Migrations;

namespace Wells.Persistence.Migrations
{
    public partial class FlnaThickness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FlnaThickness",
                table: "Measurements",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlnaThickness",
                table: "Measurements");
        }
    }
}
