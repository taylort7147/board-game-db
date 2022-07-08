using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameDB.Migrations
{
    public partial class RedoEntityRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameGameType_Game_GameId",
                table: "GameGameType");

            migrationBuilder.DropForeignKey(
                name: "FK_GameGameType_GameType_GameTypeId",
                table: "GameGameType");

            migrationBuilder.DropForeignKey(
                name: "FK_GameMechanic_Game_GameId",
                table: "GameMechanic");

            migrationBuilder.DropForeignKey(
                name: "FK_GameMechanic_Mechanic_MechanicId",
                table: "GameMechanic");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayStyle_Game_GameId",
                table: "GamePlayStyle");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayStyle_PlayStyle_PlayStyleId",
                table: "GamePlayStyle");

            migrationBuilder.DropForeignKey(
                name: "FK_GameType_Game_GameId",
                table: "GameType");

            migrationBuilder.DropForeignKey(
                name: "FK_Mechanic_Game_GameId",
                table: "Mechanic");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayStyle_Game_GameId",
                table: "PlayStyle");

            migrationBuilder.DropIndex(
                name: "IX_PlayStyle_GameId",
                table: "PlayStyle");

            migrationBuilder.DropIndex(
                name: "IX_Mechanic_GameId",
                table: "Mechanic");

            migrationBuilder.DropIndex(
                name: "IX_GameType_GameId",
                table: "GameType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GamePlayStyle",
                table: "GamePlayStyle");

            migrationBuilder.DropIndex(
                name: "IX_GamePlayStyle_GameId",
                table: "GamePlayStyle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameMechanic",
                table: "GameMechanic");

            migrationBuilder.DropIndex(
                name: "IX_GameMechanic_GameId",
                table: "GameMechanic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameGameType",
                table: "GameGameType");

            migrationBuilder.DropIndex(
                name: "IX_GameGameType_GameId",
                table: "GameGameType");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "PlayStyle");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Mechanic");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "GameType");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GamePlayStyle");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GameMechanic");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GameGameType");

            migrationBuilder.RenameColumn(
                name: "PlayStyleId",
                table: "GamePlayStyle",
                newName: "PlayStylesId");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "GamePlayStyle",
                newName: "GamesId");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlayStyle_PlayStyleId",
                table: "GamePlayStyle",
                newName: "IX_GamePlayStyle_PlayStylesId");

            migrationBuilder.RenameColumn(
                name: "MechanicId",
                table: "GameMechanic",
                newName: "MechanicsId");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "GameMechanic",
                newName: "GamesId");

            migrationBuilder.RenameIndex(
                name: "IX_GameMechanic_MechanicId",
                table: "GameMechanic",
                newName: "IX_GameMechanic_MechanicsId");

            migrationBuilder.RenameColumn(
                name: "GameTypeId",
                table: "GameGameType",
                newName: "GamesId");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "GameGameType",
                newName: "GameTypesId");

            migrationBuilder.RenameIndex(
                name: "IX_GameGameType_GameTypeId",
                table: "GameGameType",
                newName: "IX_GameGameType_GamesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GamePlayStyle",
                table: "GamePlayStyle",
                columns: new[] { "GamesId", "PlayStylesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameMechanic",
                table: "GameMechanic",
                columns: new[] { "GamesId", "MechanicsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameGameType",
                table: "GameGameType",
                columns: new[] { "GameTypesId", "GamesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GameGameType_Game_GamesId",
                table: "GameGameType",
                column: "GamesId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameGameType_GameType_GameTypesId",
                table: "GameGameType",
                column: "GameTypesId",
                principalTable: "GameType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameMechanic_Game_GamesId",
                table: "GameMechanic",
                column: "GamesId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameMechanic_Mechanic_MechanicsId",
                table: "GameMechanic",
                column: "MechanicsId",
                principalTable: "Mechanic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayStyle_Game_GamesId",
                table: "GamePlayStyle",
                column: "GamesId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayStyle_PlayStyle_PlayStylesId",
                table: "GamePlayStyle",
                column: "PlayStylesId",
                principalTable: "PlayStyle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameGameType_Game_GamesId",
                table: "GameGameType");

            migrationBuilder.DropForeignKey(
                name: "FK_GameGameType_GameType_GameTypesId",
                table: "GameGameType");

            migrationBuilder.DropForeignKey(
                name: "FK_GameMechanic_Game_GamesId",
                table: "GameMechanic");

            migrationBuilder.DropForeignKey(
                name: "FK_GameMechanic_Mechanic_MechanicsId",
                table: "GameMechanic");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayStyle_Game_GamesId",
                table: "GamePlayStyle");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayStyle_PlayStyle_PlayStylesId",
                table: "GamePlayStyle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GamePlayStyle",
                table: "GamePlayStyle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameMechanic",
                table: "GameMechanic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameGameType",
                table: "GameGameType");

            migrationBuilder.RenameColumn(
                name: "PlayStylesId",
                table: "GamePlayStyle",
                newName: "PlayStyleId");

            migrationBuilder.RenameColumn(
                name: "GamesId",
                table: "GamePlayStyle",
                newName: "GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlayStyle_PlayStylesId",
                table: "GamePlayStyle",
                newName: "IX_GamePlayStyle_PlayStyleId");

            migrationBuilder.RenameColumn(
                name: "MechanicsId",
                table: "GameMechanic",
                newName: "MechanicId");

            migrationBuilder.RenameColumn(
                name: "GamesId",
                table: "GameMechanic",
                newName: "GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameMechanic_MechanicsId",
                table: "GameMechanic",
                newName: "IX_GameMechanic_MechanicId");

            migrationBuilder.RenameColumn(
                name: "GamesId",
                table: "GameGameType",
                newName: "GameTypeId");

            migrationBuilder.RenameColumn(
                name: "GameTypesId",
                table: "GameGameType",
                newName: "GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameGameType_GamesId",
                table: "GameGameType",
                newName: "IX_GameGameType_GameTypeId");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "PlayStyle",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Mechanic",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "GameType",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GamePlayStyle",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GameMechanic",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GameGameType",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GamePlayStyle",
                table: "GamePlayStyle",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameMechanic",
                table: "GameMechanic",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameGameType",
                table: "GameGameType",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlayStyle_GameId",
                table: "PlayStyle",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Mechanic_GameId",
                table: "Mechanic",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameType_GameId",
                table: "GameType",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayStyle_GameId",
                table: "GamePlayStyle",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameMechanic_GameId",
                table: "GameMechanic",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameGameType_GameId",
                table: "GameGameType",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameGameType_Game_GameId",
                table: "GameGameType",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameGameType_GameType_GameTypeId",
                table: "GameGameType",
                column: "GameTypeId",
                principalTable: "GameType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameMechanic_Game_GameId",
                table: "GameMechanic",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameMechanic_Mechanic_MechanicId",
                table: "GameMechanic",
                column: "MechanicId",
                principalTable: "Mechanic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayStyle_Game_GameId",
                table: "GamePlayStyle",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayStyle_PlayStyle_PlayStyleId",
                table: "GamePlayStyle",
                column: "PlayStyleId",
                principalTable: "PlayStyle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameType_Game_GameId",
                table: "GameType",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanic_Game_GameId",
                table: "Mechanic",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayStyle_Game_GameId",
                table: "PlayStyle",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id");
        }
    }
}
