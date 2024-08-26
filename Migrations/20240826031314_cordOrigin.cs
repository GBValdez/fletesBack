using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fletesProyect.Migrations
{
    /// <inheritdoc />
    public partial class cordOrigin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "originCoord",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_catalogues_gasolineTypeId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_gasolineTypeId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "originCoord",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "gasolineTypeId",
                table: "Drivers");
        }
    }
}
