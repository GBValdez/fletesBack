using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fletesProyect.Migrations
{
    /// <inheritdoc />
    public partial class typeVehicleRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "typeVehicleId",
                table: "modelGasolines",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_modelGasolines_typeVehicleId",
                table: "modelGasolines",
                column: "typeVehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_modelGasolines_catalogues_typeVehicleId",
                table: "modelGasolines",
                column: "typeVehicleId",
                principalTable: "catalogues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_modelGasolines_catalogues_typeVehicleId",
                table: "modelGasolines");

            migrationBuilder.DropIndex(
                name: "IX_modelGasolines_typeVehicleId",
                table: "modelGasolines");

            migrationBuilder.DropColumn(
                name: "typeVehicleId",
                table: "modelGasolines");
        }
    }
}
