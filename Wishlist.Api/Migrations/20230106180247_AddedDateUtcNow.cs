using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wishlist.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedDateUtcNow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Wishlists",
                keyColumn: "Id",
                keyValue: 1,
                column: "AddedDate",
                value: new DateTime(2023, 1, 6, 18, 2, 47, 134, DateTimeKind.Utc).AddTicks(9133));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Wishlists",
                keyColumn: "Id",
                keyValue: 1,
                column: "AddedDate",
                value: new DateTime(2023, 1, 6, 19, 1, 31, 347, DateTimeKind.Local).AddTicks(246));
        }
    }
}
