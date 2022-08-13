using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaBookingSystem.Persistence.Migrations
{
    public partial class change_form_double_to_string_in_ImdbRating_property : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImdbRating",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ImdbRating",
                table: "Movies",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
