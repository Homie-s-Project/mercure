using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mercure.API.Migrations
{
    public partial class OrderAddSessionIdAndFixLongTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "OrderPrice",
                table: "Orders",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Quantity",
                table: "OrderProducts",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "OrderPrice",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrderProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
