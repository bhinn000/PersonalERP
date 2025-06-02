using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalERP.Migrations
{
    /// <inheritdoc />
    public partial class AddedNeccessaryColumninCraftsOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArtName",
                table: "CraftsOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "CraftsOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtName",
                table: "CraftsOrders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CraftsOrders");
        }
    }
}
