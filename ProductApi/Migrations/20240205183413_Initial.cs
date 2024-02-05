using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductApi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EAN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ManufacturerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    LogisticUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.UniqueConstraint("AK_Products_SKU", x => x.SKU);
                });

            migrationBuilder.CreateTable(
                name: "StgInventories",
                columns: table => new
                {
                    product_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sku = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    qty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    manufacturer_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    manufacturer_ref_num = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shipping = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shipping_cost = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "StgPrices",
                columns: table => new
                {
                    Column1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Column2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Column3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Column4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Column5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Column6 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "StgProducts",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SKU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reference_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    can_be_returned = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    producer_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_wire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shipping = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    package_size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    available = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    logistic_height = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    logistic_width = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    logistic_length = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    logistic_weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_vendor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    available_in_parcel_locker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    default_image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProductSKU = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NetPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_Products_ProductSKU",
                        column: x => x.ProductSKU,
                        principalTable: "Products",
                        principalColumn: "SKU",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId",
                table: "Inventories",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prices_ProductSKU",
                table: "Prices",
                column: "ProductSKU",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "StgInventories");

            migrationBuilder.DropTable(
                name: "StgPrices");

            migrationBuilder.DropTable(
                name: "StgProducts");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
