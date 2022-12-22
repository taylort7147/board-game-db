using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameDB.Migrations
{
    public partial class AddDisplayNameToSiteSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "SiteSetting",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                collation: "NOCASE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "SiteSetting");
        }
    }
}
