using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fletesProyect.Migrations
{
    /// <inheritdoc />
    public partial class country : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "countryId",
                table: "stations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "countryOptId",
                table: "Drivers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_stations_countryId",
                table: "stations",
                column: "countryId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_countryOptId",
                table: "Drivers",
                column: "countryOptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_catalogues_countryOptId",
                table: "Drivers",
                column: "countryOptId",
                principalTable: "catalogues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_stations_catalogues_countryId",
                table: "stations",
                column: "countryId",
                principalTable: "catalogues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_catalogues_countryOptId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_stations_catalogues_countryId",
                table: "stations");

            migrationBuilder.DropIndex(
                name: "IX_stations_countryId",
                table: "stations");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_countryOptId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "countryId",
                table: "stations");

            migrationBuilder.DropColumn(
                name: "countryOptId",
                table: "Drivers");
        }
    }
}
