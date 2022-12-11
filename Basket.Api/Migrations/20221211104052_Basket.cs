using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Basket.Api.Migrations
{
    /// <inheritdoc />
    public partial class Basket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BasketItems",
                keyColumn: "BasketItemId",
                keyValue: 1,
                columns: new[] { "Amount", "Price" },
                values: new object[] { 2, 3498m });

            migrationBuilder.InsertData(
                table: "BasketItems",
                columns: new[] { "BasketItemId", "Amount", "Price", "ProductId", "UserId" },
                values: new object[,]
                {
                    { 2, 1, 1749m, 139120876, 1 },
                    { 3, 1, 1749m, 138536499, 1 },
                    { 4, 1, 1749m, 139120876, 2 },
                    { 5, 1, 1749m, 138536499, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BasketItems",
                keyColumn: "BasketItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BasketItems",
                keyColumn: "BasketItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BasketItems",
                keyColumn: "BasketItemId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BasketItems",
                keyColumn: "BasketItemId",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "BasketItems",
                keyColumn: "BasketItemId",
                keyValue: 1,
                columns: new[] { "Amount", "Price" },
                values: new object[] { 1, 1749m });
        }
    }
}
