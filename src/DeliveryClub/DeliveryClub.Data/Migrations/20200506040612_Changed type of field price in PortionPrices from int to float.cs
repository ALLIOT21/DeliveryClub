using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryClub.Data.Migrations
{
    public partial class ChangedtypeoffieldpriceinPortionPricesfrominttofloat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "PortionPrices",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "PortionPrices",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
