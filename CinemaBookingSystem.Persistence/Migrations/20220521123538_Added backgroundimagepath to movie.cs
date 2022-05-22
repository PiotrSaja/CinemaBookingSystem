using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaBookingSystem.Persistence.Migrations
{
    public partial class Addedbackgroundimagepathtomovie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundImagePath",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundImagePath",
                table: "Movies");
        }
    }
}
