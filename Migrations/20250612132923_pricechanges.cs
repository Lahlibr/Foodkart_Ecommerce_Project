using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodkart.Migrations
{
    /// <inheritdoc />
    public partial class pricechanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Real_Price",
                table: "Products",
                newName: "RealPrice");

            migrationBuilder.RenameColumn(
                name: "Offer_Price",
                table: "Products",
                newName: "OfferPrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RealPrice",
                table: "Products",
                newName: "Real_Price");

            migrationBuilder.RenameColumn(
                name: "OfferPrice",
                table: "Products",
                newName: "Offer_Price");
        }
    }
}
