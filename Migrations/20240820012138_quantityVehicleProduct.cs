using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fletesProyect.Migrations
{
    /// <inheritdoc />
    public partial class quantityVehicleProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "quantity",
                table: "VehicleProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "quantity",
                table: "VehicleProducts");
        }
    }
}
