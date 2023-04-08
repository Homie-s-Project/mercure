using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mercure.API.Migrations
{
    public partial class TypeFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProduct_Categories_CategoriessCategoryId",
                table: "CategoryProduct");

            migrationBuilder.RenameColumn(
                name: "CategoriessCategoryId",
                table: "CategoryProduct",
                newName: "CategoriesCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProduct_Categories_CategoriesCategoryId",
                table: "CategoryProduct",
                column: "CategoriesCategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProduct_Categories_CategoriesCategoryId",
                table: "CategoryProduct");

            migrationBuilder.RenameColumn(
                name: "CategoriesCategoryId",
                table: "CategoryProduct",
                newName: "CategoriessCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProduct_Categories_CategoriessCategoryId",
                table: "CategoryProduct",
                column: "CategoriessCategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
