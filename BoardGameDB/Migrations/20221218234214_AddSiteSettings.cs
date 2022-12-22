using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameDB.Migrations
{
    public partial class AddSiteSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteSetting",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    Value = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSetting", x => x.Name);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteSetting");
        }
    }
}
