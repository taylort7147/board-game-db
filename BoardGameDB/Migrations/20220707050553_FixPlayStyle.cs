using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameDB.Migrations
{
    public partial class FixPlayStyle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "PlayStyle",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayStyle_GameId",
                table: "PlayStyle",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayStyle_Game_GameId",
                table: "PlayStyle",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayStyle_Game_GameId",
                table: "PlayStyle");

            migrationBuilder.DropIndex(
                name: "IX_PlayStyle_GameId",
                table: "PlayStyle");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "PlayStyle");
        }
    }
}
