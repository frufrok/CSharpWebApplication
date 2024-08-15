using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSharpWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "ProductToProductStorage",
                table: "ProductStorages");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStorages_Products_ProductID",
                table: "ProductStorages",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStorages_Products_ProductID",
                table: "ProductStorages");

            migrationBuilder.AddForeignKey(
                name: "ProductToProductStorage",
                table: "ProductStorages",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
