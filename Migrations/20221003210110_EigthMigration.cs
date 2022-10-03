using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gameSwapCSharp.Migrations
{
    public partial class EigthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Games",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Games",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Games");
        }
    }
}
