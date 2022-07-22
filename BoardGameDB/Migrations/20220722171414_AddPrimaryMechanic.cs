using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameDB.Migrations
{
    public partial class AddPrimaryMechanic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrimaryMechanicId",
                table: "Game",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Game_PrimaryMechanicId",
                table: "Game",
                column: "PrimaryMechanicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Mechanic_PrimaryMechanicId",
                table: "Game",
                column: "PrimaryMechanicId",
                principalTable: "Mechanic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Mechanic_PrimaryMechanicId",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Game_PrimaryMechanicId",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "PrimaryMechanicId",
                table: "Game");
        }
    }
}
