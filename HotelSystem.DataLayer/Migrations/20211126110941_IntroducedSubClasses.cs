using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelSystem.DataLayer.Migrations
{
    public partial class IntroducedSubClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "People",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "People",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "People",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "People");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "People");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "People");
        }
    }
}
