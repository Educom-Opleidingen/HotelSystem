using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelSystem.DataLayer.Migrations
{
    public partial class renamed_ids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Rooms_RoomId",
                table: "People");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_People",
                table: "People");

            migrationBuilder.RenameColumn(
                name: "RoomId", 
                table: "Rooms", 
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "People",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_People",
                table: "People",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Rooms_RoomId",
                table: "People",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Rooms_RoomId",
                table: "People");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_People",
                table: "People");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Rooms",
                newName: "RoomId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "People",
                newName: "PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "RoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_People",
                table: "People",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Rooms_RoomId",
                table: "People",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
