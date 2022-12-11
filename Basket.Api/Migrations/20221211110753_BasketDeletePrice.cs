using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basket.Api.Migrations
{
    /// <inheritdoc />
    public partial class BasketDeletePrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "BasketItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "BasketItems",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "BasketItems",
                keyColumn: "BasketItemId",
                keyValue: 1,
                column: "Price",
                value: 3498m);

            migrationBuilder.UpdateData(
                table: "BasketItems",
                keyColumn: "BasketItemId",
                keyValue: 2,
                column: "Price",
                value: 1749m);

            migrationBuilder.UpdateData(
                table: "BasketItems",
                keyColumn: "BasketItemId",
                keyValue: 3,
                column: "Price",
                value: 1749m);

            migrationBuilder.UpdateData(
                table: "BasketItems",
                keyColumn: "BasketItemId",
                keyValue: 4,
                column: "Price",
                value: 1749m);

            migrationBuilder.UpdateData(
                table: "BasketItems",
                keyColumn: "BasketItemId",
                keyValue: 5,
                column: "Price",
                value: 1749m);
        }
    }
}
