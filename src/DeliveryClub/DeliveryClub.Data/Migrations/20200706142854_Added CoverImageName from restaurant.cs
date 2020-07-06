using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryClub.Data.Migrations
{
    public partial class AddedCoverImageNamefromrestaurant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImageName",
                table: "Restaurants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImageName",
                table: "Restaurants");
        }
    }
}
