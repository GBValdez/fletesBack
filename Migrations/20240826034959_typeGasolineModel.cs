using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fletesProyect.Migrations
{
    /// <inheritdoc />
    public partial class typeGasolineModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_catalogues_gasolineTypeId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_gasolineTypeId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "gasolineTypeId",
                table: "Drivers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "gasolineTypeId",
                table: "Drivers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_gasolineTypeId",
                table: "Drivers",
                column: "gasolineTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_catalogues_gasolineTypeId",
                table: "Drivers",
                column: "gasolineTypeId",
                principalTable: "catalogues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
