using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gameSwapCSharp.Migrations
{
    public partial class SeventhMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "GameReceived",
                table: "Swaps",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameReceived",
                table: "Swaps");
        }
    }
}
