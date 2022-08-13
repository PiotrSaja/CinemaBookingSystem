using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaBookingSystem.Persistence.Migrations
{
    public partial class AddedNumberOfRowsAndColumnsInCinemaHall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfColumns",
                table: "CinemaHalls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRows",
                table: "CinemaHalls",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfColumns",
                table: "CinemaHalls");

            migrationBuilder.DropColumn(
                name: "NumberOfRows",
                table: "CinemaHalls");
        }
    }
}
