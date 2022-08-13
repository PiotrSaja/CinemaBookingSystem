using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaBookingSystem.Persistence.Migrations
{
    public partial class add_phone_number_to_personal_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DirectorName_PhoneNumber",
                table: "Directors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalName_PhoneNumber",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActorName_PhoneNumber",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DirectorName_PhoneNumber",
                table: "Directors");

            migrationBuilder.DropColumn(
                name: "PersonalName_PhoneNumber",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ActorName_PhoneNumber",
                table: "Actors");
        }
    }
}
