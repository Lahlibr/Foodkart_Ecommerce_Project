using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodkart.Migrations
{
    /// <inheritdoc />
    public partial class smchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Products",
                newName: "ProductName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Products",
                newName: "Title");
        }
    }
}
