using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalERP.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedCraftsOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BillPaymentCredits_CraftsOrderId",
                table: "BillPaymentCredits");

            migrationBuilder.AddColumn<int>(
                name: "BillPaymentId",
                table: "CraftsOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillPaymentCredits_CraftsOrderId",
                table: "BillPaymentCredits",
                column: "CraftsOrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BillPaymentCredits_CraftsOrderId",
                table: "BillPaymentCredits");

            migrationBuilder.DropColumn(
                name: "BillPaymentId",
                table: "CraftsOrders");

            migrationBuilder.CreateIndex(
                name: "IX_BillPaymentCredits_CraftsOrderId",
                table: "BillPaymentCredits",
                column: "CraftsOrderId");
        }
    }
}
