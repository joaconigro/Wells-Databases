using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wells.CorePersistence.Migrations
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
                    Name = table.Column<string>(nullable: true),
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
                name: "ChemicalAnalysis",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    WellId = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    SampleOf = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    GRO = table.Column<double>(nullable: true),
                    DRO = table.Column<double>(nullable: true),
                    MRO = table.Column<double>(nullable: true),
                    Benzene = table.Column<double>(nullable: true),
                    Tolueno = table.Column<double>(nullable: true),
                    Ethylbenzene = table.Column<double>(nullable: true),
                    Xylenes = table.Column<double>(nullable: true),
                    C6_C8 = table.Column<double>(nullable: true),
                    C8_C10 = table.Column<double>(nullable: true),
                    C10_C12 = table.Column<double>(nullable: true),
                    C12_C16 = table.Column<double>(nullable: true),
                    C16_C21 = table.Column<double>(nullable: true),
                    C21_C35 = table.Column<double>(nullable: true),
                    C17_Pristano = table.Column<double>(nullable: true),
                    C18_Fitano = table.Column<double>(nullable: true),
                    RealDensity = table.Column<double>(nullable: true),
                    DynamicViscosity = table.Column<double>(nullable: true),
                    WellId1 = table.Column<string>(nullable: true),
                    Humidity = table.Column<double>(nullable: true),
                    PH = table.Column<double>(nullable: true),
                    SoilAnalysis_GRO = table.Column<double>(nullable: true),
                    SoilAnalysis_DRO = table.Column<double>(nullable: true),
                    SoilAnalysis_MRO = table.Column<double>(nullable: true),
                    TotalHydrocarbons_EPA8015 = table.Column<double>(nullable: true),
                    TotalHydrocarbons_TNRCC1005 = table.Column<double>(nullable: true),
                    OilsAndFats = table.Column<double>(nullable: true),
                    C6_C8Aliphatic = table.Column<double>(nullable: true),
                    C8_C10Aliphatic = table.Column<double>(nullable: true),
                    C10_C12Aliphatic = table.Column<double>(nullable: true),
                    C12_C16Aliphatic = table.Column<double>(nullable: true),
                    C16_C21Aliphatic = table.Column<double>(nullable: true),
                    C21_C35Aliphatic = table.Column<double>(nullable: true),
                    C6_C8Aromatic = table.Column<double>(nullable: true),
                    C8_C10Aromatic = table.Column<double>(nullable: true),
                    C10_C12Aromatic = table.Column<double>(nullable: true),
                    C12_C16Aromatic = table.Column<double>(nullable: true),
                    C16_C21Aromatic = table.Column<double>(nullable: true),
                    C21_C35Aromatic = table.Column<double>(nullable: true),
                    SoilAnalysis_Benzene = table.Column<double>(nullable: true),
                    SoilAnalysis_Tolueno = table.Column<double>(nullable: true),
                    SoilAnalysis_Ethylbenzene = table.Column<double>(nullable: true),
                    XyleneO = table.Column<double>(nullable: true),
                    XylenePM = table.Column<double>(nullable: true),
                    TotalXylene = table.Column<double>(nullable: true),
                    Naphthalene = table.Column<double>(nullable: true),
                    Acenafthene = table.Column<double>(nullable: true),
                    Acenaphthylene = table.Column<double>(nullable: true),
                    Fluorene = table.Column<double>(nullable: true),
                    Anthracene = table.Column<double>(nullable: true),
                    Fenanthrene = table.Column<double>(nullable: true),
                    Fluoranthene = table.Column<double>(nullable: true),
                    Pyrene = table.Column<double>(nullable: true),
                    Crysene = table.Column<double>(nullable: true),
                    BenzoAAnthracene = table.Column<double>(nullable: true),
                    BenzoAPyrene = table.Column<double>(nullable: true),
                    BenzoBFluoranthene = table.Column<double>(nullable: true),
                    BenzoGHIPerylene = table.Column<double>(nullable: true),
                    BenzoKFluoranthene = table.Column<double>(nullable: true),
                    DibenzoAHAnthracene = table.Column<double>(nullable: true),
                    Indeno123CDPyrene = table.Column<double>(nullable: true),
                    Arsenic = table.Column<double>(nullable: true),
                    Cadmium = table.Column<double>(nullable: true),
                    Copper = table.Column<double>(nullable: true),
                    TotalChrome = table.Column<double>(nullable: true),
                    Mercury = table.Column<double>(nullable: true),
                    Nickel = table.Column<double>(nullable: true),
                    Lead = table.Column<double>(nullable: true),
                    Selenium = table.Column<double>(nullable: true),
                    Zinc = table.Column<double>(nullable: true),
                    SoilAnalysis_WellId1 = table.Column<string>(nullable: true),
                    WaterAnalysis_PH = table.Column<double>(nullable: true),
                    Conductivity = table.Column<double>(nullable: true),
                    DryWaste = table.Column<double>(nullable: true),
                    BicarbonateAlkalinity = table.Column<double>(nullable: true),
                    CarbonateAlkalinity = table.Column<double>(nullable: true),
                    Chlorides = table.Column<double>(nullable: true),
                    Nitrates = table.Column<double>(nullable: true),
                    Sulfates = table.Column<double>(nullable: true),
                    Calcium = table.Column<double>(nullable: true),
                    Magnesium = table.Column<double>(nullable: true),
                    TotalSulfur = table.Column<double>(nullable: true),
                    Potassium = table.Column<double>(nullable: true),
                    Sodium = table.Column<double>(nullable: true),
                    Fluorides = table.Column<double>(nullable: true),
                    WaterAnalysis_DRO = table.Column<double>(nullable: true),
                    WaterAnalysis_GRO = table.Column<double>(nullable: true),
                    WaterAnalysis_MRO = table.Column<double>(nullable: true),
                    WaterAnalysis_TotalHydrocarbons_EPA8015 = table.Column<double>(nullable: true),
                    WaterAnalysis_TotalHydrocarbons_TNRCC1005 = table.Column<double>(nullable: true),
                    WaterAnalysis_Benzene = table.Column<double>(nullable: true),
                    WaterAnalysis_Tolueno = table.Column<double>(nullable: true),
                    WaterAnalysis_Ethylbenzene = table.Column<double>(nullable: true),
                    WaterAnalysis_XyleneO = table.Column<double>(nullable: true),
                    WaterAnalysis_XylenePM = table.Column<double>(nullable: true),
                    WaterAnalysis_TotalXylene = table.Column<double>(nullable: true),
                    WaterAnalysis_Naphthalene = table.Column<double>(nullable: true),
                    WaterAnalysis_Acenafthene = table.Column<double>(nullable: true),
                    WaterAnalysis_Acenaphthylene = table.Column<double>(nullable: true),
                    WaterAnalysis_Fluorene = table.Column<double>(nullable: true),
                    WaterAnalysis_Anthracene = table.Column<double>(nullable: true),
                    WaterAnalysis_Fenanthrene = table.Column<double>(nullable: true),
                    WaterAnalysis_Fluoranthene = table.Column<double>(nullable: true),
                    WaterAnalysis_Pyrene = table.Column<double>(nullable: true),
                    WaterAnalysis_BenzoAAnthracene = table.Column<double>(nullable: true),
                    WaterAnalysis_Crysene = table.Column<double>(nullable: true),
                    WaterAnalysis_BenzoAPyrene = table.Column<double>(nullable: true),
                    WaterAnalysis_BenzoBFluoranthene = table.Column<double>(nullable: true),
                    WaterAnalysis_BenzoGHIPerylene = table.Column<double>(nullable: true),
                    WaterAnalysis_BenzoKFluoranthene = table.Column<double>(nullable: true),
                    WaterAnalysis_DibenzoAHAnthracene = table.Column<double>(nullable: true),
                    WaterAnalysis_Indeno123CDPyrene = table.Column<double>(nullable: true),
                    WaterAnalysis_Arsenic = table.Column<double>(nullable: true),
                    WaterAnalysis_Cadmium = table.Column<double>(nullable: true),
                    Cobalt = table.Column<double>(nullable: true),
                    WaterAnalysis_Copper = table.Column<double>(nullable: true),
                    WaterAnalysis_TotalChrome = table.Column<double>(nullable: true),
                    WaterAnalysis_Mercury = table.Column<double>(nullable: true),
                    WaterAnalysis_Nickel = table.Column<double>(nullable: true),
                    WaterAnalysis_Lead = table.Column<double>(nullable: true),
                    WaterAnalysis_Selenium = table.Column<double>(nullable: true),
                    WaterAnalysis_Zinc = table.Column<double>(nullable: true),
                    WaterAnalysis_WellId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChemicalAnalysis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChemicalAnalysis_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChemicalAnalysis_Wells_WellId1",
                        column: x => x.WellId1,
                        principalTable: "Wells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChemicalAnalysis_Wells_SoilAnalysis_WellId1",
                        column: x => x.SoilAnalysis_WellId1,
                        principalTable: "Wells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChemicalAnalysis_Wells_WaterAnalysis_WellId1",
                        column: x => x.WaterAnalysis_WellId1,
                        principalTable: "Wells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    WellId = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    FLNADepth = table.Column<double>(nullable: false),
                    WaterDepth = table.Column<double>(nullable: false),
                    Caudal = table.Column<double>(nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_ChemicalAnalysis_WellId",
                table: "ChemicalAnalysis",
                column: "WellId");

            migrationBuilder.CreateIndex(
                name: "IX_ChemicalAnalysis_WellId1",
                table: "ChemicalAnalysis",
                column: "WellId1");

            migrationBuilder.CreateIndex(
                name: "IX_ChemicalAnalysis_SoilAnalysis_WellId1",
                table: "ChemicalAnalysis",
                column: "SoilAnalysis_WellId1");

            migrationBuilder.CreateIndex(
                name: "IX_ChemicalAnalysis_WaterAnalysis_WellId1",
                table: "ChemicalAnalysis",
                column: "WaterAnalysis_WellId1");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalFiles_WellId",
                table: "ExternalFiles",
                column: "WellId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_WellId",
                table: "Measurements",
                column: "WellId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChemicalAnalysis");

            migrationBuilder.DropTable(
                name: "ExternalFiles");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "Precipitations");

            migrationBuilder.DropTable(
                name: "Wells");
        }
    }
}
