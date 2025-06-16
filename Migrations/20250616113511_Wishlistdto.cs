using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodkart.Migrations
{
    /// <inheritdoc />
    public partial class Wishlistdto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Wishlists",
                newName: "WishlistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WishlistId",
                table: "Wishlists",
                newName: "Id");
        }
    }
}
