using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectsInfo.Migrations
{
    public partial class PriceOfHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DevelopmentHourPrice",
                table: "Projects",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TestingHourPrice",
                table: "Projects",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DevelopmentHourPrice",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TestingHourPrice",
                table: "Projects");
        }
    }
}
