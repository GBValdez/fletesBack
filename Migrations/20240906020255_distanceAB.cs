using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace fletesProyect.Migrations
{
    /// <inheritdoc />
    public partial class distanceAB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "routeStations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    stationAId = table.Column<long>(type: "bigint", nullable: false),
                    stationBId = table.Column<long>(type: "bigint", nullable: false),
                    distance = table.Column<double>(type: "double precision", nullable: false),
                    userUpdateId = table.Column<string>(type: "text", nullable: true),
                    deleteAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routeStations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_routeStations_AspNetUsers_userUpdateId",
                        column: x => x.userUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_routeStations_stations_stationAId",
                        column: x => x.stationAId,
                        principalTable: "stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_routeStations_stations_stationBId",
                        column: x => x.stationBId,
                        principalTable: "stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_routeStations_stationAId",
                table: "routeStations",
                column: "stationAId");

            migrationBuilder.CreateIndex(
                name: "IX_routeStations_stationBId",
                table: "routeStations",
                column: "stationBId");

            migrationBuilder.CreateIndex(
                name: "IX_routeStations_userUpdateId",
                table: "routeStations",
                column: "userUpdateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "routeStations");
        }
    }
}
