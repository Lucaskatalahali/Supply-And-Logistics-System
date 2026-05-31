using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Supply_And_Logistics_System.Migrations
{
    /// <inheritdoc />
    public partial class CarrierNameToShipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarrierName",
                table: "Shipments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarrierName",
                table: "Shipments");
        }
    }
}
