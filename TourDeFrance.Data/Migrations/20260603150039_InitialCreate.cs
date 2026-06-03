using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourDeFrance.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deelnemers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naam = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    AangemaaktOp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deelnemers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Etappes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EtappeNummer = table.Column<int>(type: "INTEGER", nullable: false),
                    Datum = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    StartLocatie = table.Column<string>(type: "TEXT", nullable: true),
                    EindLocatie = table.Column<string>(type: "TEXT", nullable: true),
                    Afstand = table.Column<decimal>(type: "TEXT", nullable: true),
                    EtappeType = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etappes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ploegen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naam = table.Column<string>(type: "TEXT", nullable: false),
                    Land = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ploegen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Renners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Startnummer = table.Column<int>(type: "INTEGER", nullable: false),
                    Voornaam = table.Column<string>(type: "TEXT", nullable: false),
                    Achternaam = table.Column<string>(type: "TEXT", nullable: false),
                    Land = table.Column<string>(type: "TEXT", nullable: true),
                    PloegId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Renners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Renners_Ploegen_PloegId",
                        column: x => x.PloegId,
                        principalTable: "Ploegen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeelnemerSelecties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeelnemerId = table.Column<int>(type: "INTEGER", nullable: false),
                    RennerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Volgorde = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeelnemerSelecties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeelnemerSelecties_Deelnemers_DeelnemerId",
                        column: x => x.DeelnemerId,
                        principalTable: "Deelnemers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeelnemerSelecties_Renners_RennerId",
                        column: x => x.RennerId,
                        principalTable: "Renners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EindKlassementen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RennerId = table.Column<int>(type: "INTEGER", nullable: false),
                    AKPositie = table.Column<int>(type: "INTEGER", nullable: true),
                    PuntenPositie = table.Column<int>(type: "INTEGER", nullable: true),
                    BergPositie = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EindKlassementen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EindKlassementen_Renners_RennerId",
                        column: x => x.RennerId,
                        principalTable: "Renners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EtappeUitslagen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EtappeId = table.Column<int>(type: "INTEGER", nullable: false),
                    RennerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Positie = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtappeUitslagen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EtappeUitslagen_Etappes_EtappeId",
                        column: x => x.EtappeId,
                        principalTable: "Etappes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EtappeUitslagen_Renners_RennerId",
                        column: x => x.RennerId,
                        principalTable: "Renners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeelnemerSelecties_DeelnemerId_RennerId",
                table: "DeelnemerSelecties",
                columns: new[] { "DeelnemerId", "RennerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeelnemerSelecties_RennerId",
                table: "DeelnemerSelecties",
                column: "RennerId");

            migrationBuilder.CreateIndex(
                name: "IX_EindKlassementen_RennerId",
                table: "EindKlassementen",
                column: "RennerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EtappeUitslagen_EtappeId_RennerId",
                table: "EtappeUitslagen",
                columns: new[] { "EtappeId", "RennerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EtappeUitslagen_RennerId",
                table: "EtappeUitslagen",
                column: "RennerId");

            migrationBuilder.CreateIndex(
                name: "IX_Renners_PloegId",
                table: "Renners",
                column: "PloegId");

            migrationBuilder.CreateIndex(
                name: "IX_Renners_Startnummer",
                table: "Renners",
                column: "Startnummer",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeelnemerSelecties");

            migrationBuilder.DropTable(
                name: "EindKlassementen");

            migrationBuilder.DropTable(
                name: "EtappeUitslagen");

            migrationBuilder.DropTable(
                name: "Deelnemers");

            migrationBuilder.DropTable(
                name: "Etappes");

            migrationBuilder.DropTable(
                name: "Renners");

            migrationBuilder.DropTable(
                name: "Ploegen");
        }
    }
}
