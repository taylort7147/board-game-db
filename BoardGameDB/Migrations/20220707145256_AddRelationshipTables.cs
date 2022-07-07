using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameDB.Migrations
{
    public partial class AddRelationshipTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameGameType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    GameTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGameType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameGameType_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGameType_GameType_GameTypeId",
                        column: x => x.GameTypeId,
                        principalTable: "GameType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameMechanic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    MechanicId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameMechanic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameMechanic_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameMechanic_Mechanic_MechanicId",
                        column: x => x.MechanicId,
                        principalTable: "Mechanic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GamePlayStyle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayStyleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayStyle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamePlayStyle_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlayStyle_PlayStyle_PlayStyleId",
                        column: x => x.PlayStyleId,
                        principalTable: "PlayStyle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameGameType_GameId",
                table: "GameGameType",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameGameType_GameTypeId",
                table: "GameGameType",
                column: "GameTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GameMechanic_GameId",
                table: "GameMechanic",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameMechanic_MechanicId",
                table: "GameMechanic",
                column: "MechanicId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayStyle_GameId",
                table: "GamePlayStyle",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayStyle_PlayStyleId",
                table: "GamePlayStyle",
                column: "PlayStyleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameGameType");

            migrationBuilder.DropTable(
                name: "GameMechanic");

            migrationBuilder.DropTable(
                name: "GamePlayStyle");
        }
    }
}
