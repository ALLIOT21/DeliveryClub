using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryClub.Data.Migrations
{
    public partial class AddedRestaurantfieldtoRestaurantOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "RestaurantOrders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantOrders_RestaurantId",
                table: "RestaurantOrders",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantOrders_Restaurants_RestaurantId",
                table: "RestaurantOrders",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantOrders_Restaurants_RestaurantId",
                table: "RestaurantOrders");

            migrationBuilder.DropIndex(
                name: "IX_RestaurantOrders_RestaurantId",
                table: "RestaurantOrders");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "RestaurantOrders");
        }
    }
}
