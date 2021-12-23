using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wells.Persistence.Migrations.SqliteMigrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Precipitations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Millimeters = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Precipitations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wells",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    X = table.Column<double>(type: "REAL", nullable: false),
                    Y = table.Column<double>(type: "REAL", nullable: false),
                    Z = table.Column<double>(type: "REAL", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Height = table.Column<double>(type: "REAL", nullable: false),
                    Bottom = table.Column<double>(type: "REAL", nullable: false),
                    WellType = table.Column<int>(type: "INTEGER", nullable: false),
                    Exists = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wells", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalFiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    WellId = table.Column<string>(type: "TEXT", nullable: false),
                    FileExtension = table.Column<string>(type: "TEXT", nullable: false),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalFiles_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlnaAnalyses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Dro = table.Column<double>(type: "REAL", nullable: false),
                    Gro = table.Column<double>(type: "REAL", nullable: false),
                    Mro = table.Column<double>(type: "REAL", nullable: false),
                    Benzene = table.Column<double>(type: "REAL", nullable: false),
                    Tolueno = table.Column<double>(type: "REAL", nullable: false),
                    Ethylbenzene = table.Column<double>(type: "REAL", nullable: false),
                    Xylenes = table.Column<double>(type: "REAL", nullable: false),
                    C6_C8 = table.Column<double>(type: "REAL", nullable: false),
                    C8_C10 = table.Column<double>(type: "REAL", nullable: false),
                    C10_C12 = table.Column<double>(type: "REAL", nullable: false),
                    C12_C16 = table.Column<double>(type: "REAL", nullable: false),
                    C16_C21 = table.Column<double>(type: "REAL", nullable: false),
                    C21_C35 = table.Column<double>(type: "REAL", nullable: false),
                    C17_Pristano = table.Column<double>(type: "REAL", nullable: false),
                    C18_Fitano = table.Column<double>(type: "REAL", nullable: false),
                    RealDensity = table.Column<double>(type: "REAL", nullable: false),
                    DynamicViscosity = table.Column<double>(type: "REAL", nullable: false),
                    WellId = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlnaAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlnaAnalyses_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    WellId = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FlnaDepth = table.Column<double>(type: "REAL", nullable: false),
                    WaterDepth = table.Column<double>(type: "REAL", nullable: false),
                    FlnaThickness = table.Column<double>(type: "REAL", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Measurements_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoilAnalyses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Humidity = table.Column<double>(type: "REAL", nullable: false),
                    Ph = table.Column<double>(type: "REAL", nullable: false),
                    Dro = table.Column<double>(type: "REAL", nullable: false),
                    Gro = table.Column<double>(type: "REAL", nullable: false),
                    Mro = table.Column<double>(type: "REAL", nullable: false),
                    TotalHydrocarbonsEpa8015 = table.Column<double>(type: "REAL", nullable: false),
                    TotalHydrocarbonsTnrcc1005 = table.Column<double>(type: "REAL", nullable: false),
                    OilsAndFats = table.Column<double>(type: "REAL", nullable: false),
                    C6_C8Aliphatic = table.Column<double>(type: "REAL", nullable: false),
                    C8_C10Aliphatic = table.Column<double>(type: "REAL", nullable: false),
                    C10_C12Aliphatic = table.Column<double>(type: "REAL", nullable: false),
                    C12_C16Aliphatic = table.Column<double>(type: "REAL", nullable: false),
                    C16_C21Aliphatic = table.Column<double>(type: "REAL", nullable: false),
                    C21_C35Aliphatic = table.Column<double>(type: "REAL", nullable: false),
                    C6_C8Aromatic = table.Column<double>(type: "REAL", nullable: false),
                    C8_C10Aromatic = table.Column<double>(type: "REAL", nullable: false),
                    C10_C12Aromatic = table.Column<double>(type: "REAL", nullable: false),
                    C12_C16Aromatic = table.Column<double>(type: "REAL", nullable: false),
                    C16_C21Aromatic = table.Column<double>(type: "REAL", nullable: false),
                    C21_C35Aromatic = table.Column<double>(type: "REAL", nullable: false),
                    Benzene = table.Column<double>(type: "REAL", nullable: false),
                    Tolueno = table.Column<double>(type: "REAL", nullable: false),
                    Ethylbenzene = table.Column<double>(type: "REAL", nullable: false),
                    XyleneO = table.Column<double>(type: "REAL", nullable: false),
                    XylenePm = table.Column<double>(type: "REAL", nullable: false),
                    TotalXylene = table.Column<double>(type: "REAL", nullable: false),
                    Naphthalene = table.Column<double>(type: "REAL", nullable: false),
                    Acenafthene = table.Column<double>(type: "REAL", nullable: false),
                    Acenaphthylene = table.Column<double>(type: "REAL", nullable: false),
                    Fluorene = table.Column<double>(type: "REAL", nullable: false),
                    Anthracene = table.Column<double>(type: "REAL", nullable: false),
                    Fenanthrene = table.Column<double>(type: "REAL", nullable: false),
                    Fluoranthene = table.Column<double>(type: "REAL", nullable: false),
                    Pyrene = table.Column<double>(type: "REAL", nullable: false),
                    Crysene = table.Column<double>(type: "REAL", nullable: false),
                    BenzoAAnthracene = table.Column<double>(type: "REAL", nullable: false),
                    BenzoAPyrene = table.Column<double>(type: "REAL", nullable: false),
                    BenzoBFluoranthene = table.Column<double>(type: "REAL", nullable: false),
                    BenzoGhiPerylene = table.Column<double>(type: "REAL", nullable: false),
                    BenzoKFluoranthene = table.Column<double>(type: "REAL", nullable: false),
                    DibenzoAhAnthracene = table.Column<double>(type: "REAL", nullable: false),
                    Indeno123CdPyrene = table.Column<double>(type: "REAL", nullable: false),
                    Arsenic = table.Column<double>(type: "REAL", nullable: false),
                    Cadmium = table.Column<double>(type: "REAL", nullable: false),
                    Copper = table.Column<double>(type: "REAL", nullable: false),
                    TotalChrome = table.Column<double>(type: "REAL", nullable: false),
                    Mercury = table.Column<double>(type: "REAL", nullable: false),
                    Nickel = table.Column<double>(type: "REAL", nullable: false),
                    Lead = table.Column<double>(type: "REAL", nullable: false),
                    Selenium = table.Column<double>(type: "REAL", nullable: false),
                    Zinc = table.Column<double>(type: "REAL", nullable: false),
                    WellId = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoilAnalyses_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaterAnalyses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Ph = table.Column<double>(type: "REAL", nullable: false),
                    Conductivity = table.Column<double>(type: "REAL", nullable: false),
                    DryWaste = table.Column<double>(type: "REAL", nullable: false),
                    BicarbonateAlkalinity = table.Column<double>(type: "REAL", nullable: false),
                    CarbonateAlkalinity = table.Column<double>(type: "REAL", nullable: false),
                    Chlorides = table.Column<double>(type: "REAL", nullable: false),
                    Nitrates = table.Column<double>(type: "REAL", nullable: false),
                    Sulfates = table.Column<double>(type: "REAL", nullable: false),
                    Calcium = table.Column<double>(type: "REAL", nullable: false),
                    Magnesium = table.Column<double>(type: "REAL", nullable: false),
                    TotalSulfur = table.Column<double>(type: "REAL", nullable: false),
                    Potassium = table.Column<double>(type: "REAL", nullable: false),
                    Sodium = table.Column<double>(type: "REAL", nullable: false),
                    Fluorides = table.Column<double>(type: "REAL", nullable: false),
                    Dro = table.Column<double>(type: "REAL", nullable: false),
                    Gro = table.Column<double>(type: "REAL", nullable: false),
                    Mro = table.Column<double>(type: "REAL", nullable: false),
                    TotalHydrocarbonsEpa8015 = table.Column<double>(type: "REAL", nullable: false),
                    TotalHydrocarbonsTnrcc1005 = table.Column<double>(type: "REAL", nullable: false),
                    Benzene = table.Column<double>(type: "REAL", nullable: false),
                    Tolueno = table.Column<double>(type: "REAL", nullable: false),
                    Ethylbenzene = table.Column<double>(type: "REAL", nullable: false),
                    XyleneO = table.Column<double>(type: "REAL", nullable: false),
                    XylenePm = table.Column<double>(type: "REAL", nullable: false),
                    TotalXylene = table.Column<double>(type: "REAL", nullable: false),
                    Naphthalene = table.Column<double>(type: "REAL", nullable: false),
                    Acenafthene = table.Column<double>(type: "REAL", nullable: false),
                    Acenaphthylene = table.Column<double>(type: "REAL", nullable: false),
                    Fluorene = table.Column<double>(type: "REAL", nullable: false),
                    Anthracene = table.Column<double>(type: "REAL", nullable: false),
                    Fenanthrene = table.Column<double>(type: "REAL", nullable: false),
                    Fluoranthene = table.Column<double>(type: "REAL", nullable: false),
                    Pyrene = table.Column<double>(type: "REAL", nullable: false),
                    BenzoAAnthracene = table.Column<double>(type: "REAL", nullable: false),
                    Crysene = table.Column<double>(type: "REAL", nullable: false),
                    BenzoAPyrene = table.Column<double>(type: "REAL", nullable: false),
                    BenzoBFluoranthene = table.Column<double>(type: "REAL", nullable: false),
                    BenzoGhiPerylene = table.Column<double>(type: "REAL", nullable: false),
                    BenzoKFluoranthene = table.Column<double>(type: "REAL", nullable: false),
                    DibenzoAhAnthracene = table.Column<double>(type: "REAL", nullable: false),
                    Indeno123CdPyrene = table.Column<double>(type: "REAL", nullable: false),
                    Arsenic = table.Column<double>(type: "REAL", nullable: false),
                    Cadmium = table.Column<double>(type: "REAL", nullable: false),
                    Cobalt = table.Column<double>(type: "REAL", nullable: false),
                    Copper = table.Column<double>(type: "REAL", nullable: false),
                    TotalChrome = table.Column<double>(type: "REAL", nullable: false),
                    Mercury = table.Column<double>(type: "REAL", nullable: false),
                    Nickel = table.Column<double>(type: "REAL", nullable: false),
                    Lead = table.Column<double>(type: "REAL", nullable: false),
                    Zinc = table.Column<double>(type: "REAL", nullable: false),
                    WellId = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterAnalyses_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalFiles_WellId",
                table: "ExternalFiles",
                column: "WellId");

            migrationBuilder.CreateIndex(
                name: "IX_FlnaAnalyses_WellId",
                table: "FlnaAnalyses",
                column: "WellId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_WellId",
                table: "Measurements",
                column: "WellId");

            migrationBuilder.CreateIndex(
                name: "IX_SoilAnalyses_WellId",
                table: "SoilAnalyses",
                column: "WellId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterAnalyses_WellId",
                table: "WaterAnalyses",
                column: "WellId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalFiles");

            migrationBuilder.DropTable(
                name: "FlnaAnalyses");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "Precipitations");

            migrationBuilder.DropTable(
                name: "SoilAnalyses");

            migrationBuilder.DropTable(
                name: "WaterAnalyses");

            migrationBuilder.DropTable(
                name: "Wells");
        }
    }
}
