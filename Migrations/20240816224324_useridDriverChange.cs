using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fletesProyect.Migrations
{
    /// <inheritdoc />
    public partial class useridDriverChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_AspNetUsers_userId1",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_userId1",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "userId1",
                table: "Drivers");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "Drivers",
                type: "text",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_userId",
                table: "Drivers",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_AspNetUsers_userId",
                table: "Drivers",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_AspNetUsers_userId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_userId",
                table: "Drivers");

            migrationBuilder.AlterColumn<long>(
                name: "userId",
                table: "Drivers",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "userId1",
                table: "Drivers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_userId1",
                table: "Drivers",
                column: "userId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_AspNetUsers_userId1",
                table: "Drivers",
                column: "userId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
