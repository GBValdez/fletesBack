using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fletesProyect.Migrations
{
    /// <inheritdoc />
    public partial class ordenVisit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "orderId",
                table: "visits",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_visits_orderId",
                table: "visits",
                column: "orderId");

            migrationBuilder.AddForeignKey(
                name: "FK_visits_Orders_orderId",
                table: "visits",
                column: "orderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_visits_Orders_orderId",
                table: "visits");

            migrationBuilder.DropIndex(
                name: "IX_visits_orderId",
                table: "visits");

            migrationBuilder.DropColumn(
                name: "orderId",
                table: "visits");
        }
    }
}
