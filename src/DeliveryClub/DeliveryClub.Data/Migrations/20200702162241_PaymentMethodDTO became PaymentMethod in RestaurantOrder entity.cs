using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryClub.Data.Migrations
{
    public partial class PaymentMethodDTObecamePaymentMethodinRestaurantOrderentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantOrders_PaymentMethods_PaymentMethodId",
                table: "RestaurantOrders");

            migrationBuilder.DropIndex(
                name: "IX_RestaurantOrders_PaymentMethodId",
                table: "RestaurantOrders");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "RestaurantOrders");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "RestaurantOrders",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "RestaurantOrders");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "RestaurantOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantOrders_PaymentMethodId",
                table: "RestaurantOrders",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantOrders_PaymentMethods_PaymentMethodId",
                table: "RestaurantOrders",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
