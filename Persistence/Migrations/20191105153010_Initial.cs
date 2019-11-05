using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wells.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Precipitations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PrecipitationDate = table.Column<DateTime>(nullable: false),
                    Millimeters = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Precipitations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wells",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    X = table.Column<double>(nullable: false),
                    Y = table.Column<double>(nullable: false),
                    Z = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Height = table.Column<double>(nullable: false),
                    Bottom = table.Column<double>(nullable: false),
                    WellType = table.Column<int>(nullable: false),
                    Exists = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wells", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalFiles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    WellId = table.Column<string>(nullable: false),
                    FileExtension = table.Column<string>(nullable: false),
                    Data = table.Column<byte[]>(nullable: false)
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
                    Id = table.Column<string>(nullable: false),
                    WellId = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Gro = table.Column<double>(nullable: false),
                    Dro = table.Column<double>(nullable: false),
                    Mro = table.Column<double>(nullable: false),
                    Benzene = table.Column<double>(nullable: false),
                    Tolueno = table.Column<double>(nullable: false),
                    Ethylbenzene = table.Column<double>(nullable: false),
                    Xylenes = table.Column<double>(nullable: false),
                    C6_C8 = table.Column<double>(nullable: false),
                    C8_C10 = table.Column<double>(nullable: false),
                    C10_C12 = table.Column<double>(nullable: false),
                    C12_C16 = table.Column<double>(nullable: false),
                    C16_C21 = table.Column<double>(nullable: false),
                    C21_C35 = table.Column<double>(nullable: false),
                    C17_Pristano = table.Column<double>(nullable: false),
                    C18_Fitano = table.Column<double>(nullable: false),
                    RealDensity = table.Column<double>(nullable: false),
                    DynamicViscosity = table.Column<double>(nullable: false)
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
                    Id = table.Column<string>(nullable: false),
                    WellId = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    FlnaDepth = table.Column<double>(nullable: false),
                    WaterDepth = table.Column<double>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
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
                    Id = table.Column<string>(nullable: false),
                    WellId = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Humidity = table.Column<double>(nullable: false),
                    Ph = table.Column<double>(nullable: false),
                    Gro = table.Column<double>(nullable: false),
                    Dro = table.Column<double>(nullable: false),
                    Mro = table.Column<double>(nullable: false),
                    TotalHydrocarbonsEpa8015 = table.Column<double>(nullable: false),
                    TotalHydrocarbonsTnrcc1005 = table.Column<double>(nullable: false),
                    OilsAndFats = table.Column<double>(nullable: false),
                    C6_C8Aliphatic = table.Column<double>(nullable: false),
                    C8_C10Aliphatic = table.Column<double>(nullable: false),
                    C10_C12Aliphatic = table.Column<double>(nullable: false),
                    C12_C16Aliphatic = table.Column<double>(nullable: false),
                    C16_C21Aliphatic = table.Column<double>(nullable: false),
                    C21_C35Aliphatic = table.Column<double>(nullable: false),
                    C6_C8Aromatic = table.Column<double>(nullable: false),
                    C8_C10Aromatic = table.Column<double>(nullable: false),
                    C10_C12Aromatic = table.Column<double>(nullable: false),
                    C12_C16Aromatic = table.Column<double>(nullable: false),
                    C16_C21Aromatic = table.Column<double>(nullable: false),
                    C21_C35Aromatic = table.Column<double>(nullable: false),
                    Benzene = table.Column<double>(nullable: false),
                    Tolueno = table.Column<double>(nullable: false),
                    Ethylbenzene = table.Column<double>(nullable: false),
                    XyleneO = table.Column<double>(nullable: false),
                    XylenePm = table.Column<double>(nullable: false),
                    TotalXylene = table.Column<double>(nullable: false),
                    Naphthalene = table.Column<double>(nullable: false),
                    Acenafthene = table.Column<double>(nullable: false),
                    Acenaphthylene = table.Column<double>(nullable: false),
                    Fluorene = table.Column<double>(nullable: false),
                    Anthracene = table.Column<double>(nullable: false),
                    Fenanthrene = table.Column<double>(nullable: false),
                    Fluoranthene = table.Column<double>(nullable: false),
                    Pyrene = table.Column<double>(nullable: false),
                    Crysene = table.Column<double>(nullable: false),
                    BenzoAAnthracene = table.Column<double>(nullable: false),
                    BenzoAPyrene = table.Column<double>(nullable: false),
                    BenzoBFluoranthene = table.Column<double>(nullable: false),
                    BenzoGhiPerylene = table.Column<double>(nullable: false),
                    BenzoKFluoranthene = table.Column<double>(nullable: false),
                    DibenzoAhAnthracene = table.Column<double>(nullable: false),
                    Indeno123CdPyrene = table.Column<double>(nullable: false),
                    Arsenic = table.Column<double>(nullable: false),
                    Cadmium = table.Column<double>(nullable: false),
                    Copper = table.Column<double>(nullable: false),
                    TotalChrome = table.Column<double>(nullable: false),
                    Mercury = table.Column<double>(nullable: false),
                    Nickel = table.Column<double>(nullable: false),
                    Lead = table.Column<double>(nullable: false),
                    Selenium = table.Column<double>(nullable: false),
                    Zinc = table.Column<double>(nullable: false)
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
                    Id = table.Column<string>(nullable: false),
                    WellId = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Ph = table.Column<double>(nullable: false),
                    Conductivity = table.Column<double>(nullable: false),
                    DryWaste = table.Column<double>(nullable: false),
                    BicarbonateAlkalinity = table.Column<double>(nullable: false),
                    CarbonateAlkalinity = table.Column<double>(nullable: false),
                    Chlorides = table.Column<double>(nullable: false),
                    Nitrates = table.Column<double>(nullable: false),
                    Sulfates = table.Column<double>(nullable: false),
                    Calcium = table.Column<double>(nullable: false),
                    Magnesium = table.Column<double>(nullable: false),
                    TotalSulfur = table.Column<double>(nullable: false),
                    Potassium = table.Column<double>(nullable: false),
                    Sodium = table.Column<double>(nullable: false),
                    Fluorides = table.Column<double>(nullable: false),
                    Dro = table.Column<double>(nullable: false),
                    Gro = table.Column<double>(nullable: false),
                    Mro = table.Column<double>(nullable: false),
                    TotalHydrocarbonsEpa8015 = table.Column<double>(nullable: false),
                    TotalHydrocarbonsTnrcc1005 = table.Column<double>(nullable: false),
                    Benzene = table.Column<double>(nullable: false),
                    Tolueno = table.Column<double>(nullable: false),
                    Ethylbenzene = table.Column<double>(nullable: false),
                    XyleneO = table.Column<double>(nullable: false),
                    XylenePm = table.Column<double>(nullable: false),
                    TotalXylene = table.Column<double>(nullable: false),
                    Naphthalene = table.Column<double>(nullable: false),
                    Acenafthene = table.Column<double>(nullable: false),
                    Acenaphthylene = table.Column<double>(nullable: false),
                    Fluorene = table.Column<double>(nullable: false),
                    Anthracene = table.Column<double>(nullable: false),
                    Fenanthrene = table.Column<double>(nullable: false),
                    Fluoranthene = table.Column<double>(nullable: false),
                    Pyrene = table.Column<double>(nullable: false),
                    BenzoAAnthracene = table.Column<double>(nullable: false),
                    Crysene = table.Column<double>(nullable: false),
                    BenzoAPyrene = table.Column<double>(nullable: false),
                    BenzoBFluoranthene = table.Column<double>(nullable: false),
                    BenzoGhiPerylene = table.Column<double>(nullable: false),
                    BenzoKFluoranthene = table.Column<double>(nullable: false),
                    DibenzoAhAnthracene = table.Column<double>(nullable: false),
                    Indeno123CdPyrene = table.Column<double>(nullable: false),
                    Arsenic = table.Column<double>(nullable: false),
                    Cadmium = table.Column<double>(nullable: false),
                    Cobalt = table.Column<double>(nullable: false),
                    Copper = table.Column<double>(nullable: false),
                    TotalChrome = table.Column<double>(nullable: false),
                    Mercury = table.Column<double>(nullable: false),
                    Nickel = table.Column<double>(nullable: false),
                    Lead = table.Column<double>(nullable: false),
                    Zinc = table.Column<double>(nullable: false)
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
