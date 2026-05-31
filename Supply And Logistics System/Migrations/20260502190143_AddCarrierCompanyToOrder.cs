using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Supply_And_Logistics_System.Migrations
{
    /// <inheritdoc />
    public partial class AddCarrierCompanyToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarrierCompany",
                table: "Orders",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarrierCompany",
                table: "Orders");
        }
    }
}
