using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Supply_And_Logistics_System.Migrations
{
    /// <inheritdoc />
    public partial class AddCompositeRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompositeProductProduct",
                columns: table => new
                {
                    ComponentsId = table.Column<int>(type: "integer", nullable: false),
                    CompositeProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompositeProductProduct", x => new { x.ComponentsId, x.CompositeProductId });
                    table.ForeignKey(
                        name: "FK_CompositeProductProduct_Products_ComponentsId",
                        column: x => x.ComponentsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompositeProductProduct_Products_CompositeProductId",
                        column: x => x.CompositeProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompositeProductProduct_CompositeProductId",
                table: "CompositeProductProduct",
                column: "CompositeProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompositeProductProduct");
        }
    }
}
