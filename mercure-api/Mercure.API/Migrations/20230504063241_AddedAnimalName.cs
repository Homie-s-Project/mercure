using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mercure.API.Migrations
{
    public partial class AddedAnimalName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnimalName",
                table: "Animals",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimalName",
                table: "Animals");
        }
    }
}
