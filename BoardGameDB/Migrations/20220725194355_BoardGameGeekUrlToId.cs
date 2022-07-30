using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameDB.Migrations
{
    public partial class BoardGameGeekUrlToId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BoardGameGeekUrl",
                table: "Game",
                newName: "BoardGameGeekId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BoardGameGeekId",
                table: "Game",
                newName: "BoardGameGeekUrl");
        }
    }
}
