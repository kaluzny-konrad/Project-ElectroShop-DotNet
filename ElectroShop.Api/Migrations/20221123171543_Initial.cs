using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ElectroShop.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    ManufacturerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManufacturerName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.ManufacturerId);
                });

            migrationBuilder.CreateTable(
                name: "ProductDescriptions",
                columns: table => new
                {
                    ProductDescriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductLongDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDescriptions", x => x.ProductDescriptionId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "ManufacturerId", "ManufacturerName" },
                values: new object[,]
                {
                    { 1, "Motorola" },
                    { 2, "Apple" }
                });

            migrationBuilder.InsertData(
                table: "ProductDescriptions",
                columns: new[] { "ProductDescriptionId", "ProductId", "ProductLongDescription", "ProductShortDescription" },
                values: new object[,]
                {
                    { 1, 139108825, "Ultrasmukły i lekki, z kinowym ekranem 6,28\", oferowany w najnowszych modnych kolorach Pantone. \r\nSystem aparatów 64 Mpx z OIS \r\nRób krystalicznie wyraźne zdjęcia w wysokiej rozdzielczości w każdym oświetleniu, \r\njednocześnie eliminując rozmycia na skutek przypadkowego poruszenia aparatem.", "Smartfon Motorola z ekranem 6,7 cala, wyświetlacz OLED. Aparat 108 Mpix, pamięć 8 GB RAM, bateria 4000mAh. Obsługuje sieć: 5G" },
                    { 2, 139108829, "Ultrasmukły i lekki, z kinowym ekranem 6,28\", oferowany w najnowszych modnych kolorach Pantone. \r\nSystem aparatów 64 Mpx z OIS \r\nRób krystalicznie wyraźne zdjęcia w wysokiej rozdzielczości w każdym oświetleniu, \r\njednocześnie eliminując rozmycia na skutek przypadkowego poruszenia aparatem.", "Smartfon Motorola z ekranem 6,7 cala, wyświetlacz OLED. Aparat 108 Mpix, pamięć 8 GB RAM, bateria 4000mAh. Obsługuje sieć: 5G" },
                    { 3, 139120876, "Ultrasmukły i lekki, z kinowym ekranem 6,28\", oferowany w najnowszych modnych kolorach Pantone. \r\nSystem aparatów 64 Mpx z OIS \r\nRób krystalicznie wyraźne zdjęcia w wysokiej rozdzielczości w każdym oświetleniu, \r\njednocześnie eliminując rozmycia na skutek przypadkowego poruszenia aparatem.", "Smartfon Motorola z ekranem 6,7 cala, wyświetlacz OLED. Aparat 108 Mpix, pamięć 8 GB RAM, bateria 4000mAh. Obsługuje sieć: 5G" },
                    { 4, 113375066, "Ultrasmukły i lekki, z kinowym ekranem 6,28\", oferowany w najnowszych modnych kolorach Pantone. \r\nSystem aparatów 64 Mpx z OIS \r\nRób krystalicznie wyraźne zdjęcia w wysokiej rozdzielczości w każdym oświetleniu, \r\njednocześnie eliminując rozmycia na skutek przypadkowego poruszenia aparatem.", "Smartfon Motorola z ekranem 6,7 cala, wyświetlacz OLED. Aparat 108 Mpix, pamięć 8 GB RAM, bateria 4000mAh. Obsługuje sieć: 5G" },
                    { 5, 138536499, "Ultrasmukły i lekki, z kinowym ekranem 6,28\", oferowany w najnowszych modnych kolorach Pantone. \r\nSystem aparatów 64 Mpx z OIS \r\nRób krystalicznie wyraźne zdjęcia w wysokiej rozdzielczości w każdym oświetleniu, \r\njednocześnie eliminując rozmycia na skutek przypadkowego poruszenia aparatem.", "Smartfon Apple z ekranem 6,1 cala, wyświetlacz Super Retina XDR. Aparat 48 Mpix, pamięć 4 GB RAM. Obsługuje sieć: 5G" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "ManufacturerId", "Price", "ProductName" },
                values: new object[,]
                {
                    { 113375066, 1, 1749f, "Motorola Edge 20 8/128GB Czarny" },
                    { 138536499, 2, 6999f, "iPhone 14 Pro" },
                    { 139108825, 1, 1749f, "Motorola Edge 30 Neo 8/128GB Czarny" },
                    { 139108829, 1, 1749f, "Motorola Edge 30 Neo 8/128GB Fioletowy" },
                    { 139120876, 1, 1749f, "Motorola Edge 30 Neo 8/128GB Srebrny" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.DropTable(
                name: "ProductDescriptions");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
