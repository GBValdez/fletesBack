using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace fletesProyect.Migrations
{
    /// <inheritdoc />
    public partial class ModelGasoliness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "modelGasolines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    gasolineTypeId = table.Column<long>(type: "bigint", nullable: false),
                    modelId = table.Column<long>(type: "bigint", nullable: false),
                    gasolineLtsKm = table.Column<float>(type: "real", nullable: false),
                    userUpdateId = table.Column<string>(type: "text", nullable: true),
                    deleteAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_modelGasolines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_modelGasolines_AspNetUsers_userUpdateId",
                        column: x => x.userUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_modelGasolines_catalogues_gasolineTypeId",
                        column: x => x.gasolineTypeId,
                        principalTable: "catalogues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_modelGasolines_catalogues_modelId",
                        column: x => x.modelId,
                        principalTable: "catalogues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_modelGasolines_gasolineTypeId",
                table: "modelGasolines",
                column: "gasolineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_modelGasolines_modelId",
                table: "modelGasolines",
                column: "modelId");

            migrationBuilder.CreateIndex(
                name: "IX_modelGasolines_userUpdateId",
                table: "modelGasolines",
                column: "userUpdateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "modelGasolines");
        }
    }
}
