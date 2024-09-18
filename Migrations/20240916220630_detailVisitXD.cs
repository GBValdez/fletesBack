using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace fletesProyect.Migrations
{
    /// <inheritdoc />
    public partial class detailVisitXD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "visitProducts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ordenDetailId = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<long>(type: "bigint", nullable: false),
                    visitId = table.Column<long>(type: "bigint", nullable: false),
                    userUpdateId = table.Column<string>(type: "text", nullable: true),
                    deleteAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visitProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_visitProducts_AspNetUsers_userUpdateId",
                        column: x => x.userUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_visitProducts_OrderDetails_ordenDetailId",
                        column: x => x.ordenDetailId,
                        principalTable: "OrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_visitProducts_visits_visitId",
                        column: x => x.visitId,
                        principalTable: "visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_visitProducts_ordenDetailId",
                table: "visitProducts",
                column: "ordenDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_visitProducts_userUpdateId",
                table: "visitProducts",
                column: "userUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_visitProducts_visitId",
                table: "visitProducts",
                column: "visitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "visitProducts");
        }
    }
}
