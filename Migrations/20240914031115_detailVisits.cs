using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fletesProyect.Migrations
{
    /// <inheritdoc />
    public partial class detailVisits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_visits_OrderDetails_ordenDetailId",
                table: "visits");

            migrationBuilder.DropIndex(
                name: "IX_visits_ordenDetailId",
                table: "visits");

            migrationBuilder.DropColumn(
                name: "ordenDetailId",
                table: "visits");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "visits");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ordenDetailId",
                table: "visits",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "visits",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_visits_ordenDetailId",
                table: "visits",
                column: "ordenDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_visits_OrderDetails_ordenDetailId",
                table: "visits",
                column: "ordenDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
