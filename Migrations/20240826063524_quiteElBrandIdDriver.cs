using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fletesProyect.Migrations
{
    /// <inheritdoc />
    public partial class quiteElBrandIdDriver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_catalogues_brandId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_brandId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "brandId",
                table: "Drivers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "brandId",
                table: "Drivers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_brandId",
                table: "Drivers",
                column: "brandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_catalogues_brandId",
                table: "Drivers",
                column: "brandId",
                principalTable: "catalogues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
