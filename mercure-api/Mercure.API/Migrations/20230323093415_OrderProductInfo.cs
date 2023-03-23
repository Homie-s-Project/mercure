using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mercure.API.Migrations
{
    public partial class OrderProductInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductTitle",
                table: "Product",
                newName: "ProductName");

            migrationBuilder.AddColumn<string>(
                name: "ProductBrandName",
                table: "Product",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Shipped",
                table: "OrderProduct",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OrderStatus",
                table: "Order",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductBrandName",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Shipped",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Product",
                newName: "ProductTitle");
        }
    }
}
