﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wells.Persistence;

namespace Wells.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20191115134959_RenamePrecipitationDate")]
    partial class RenamePrecipitationDate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Wells.Model.ExternalFile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WellId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("WellId");

                    b.ToTable("ExternalFiles");
                });

            modelBuilder.Entity("Wells.Model.FlnaAnalysis", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Benzene")
                        .HasColumnType("float");

                    b.Property<double>("C10_C12")
                        .HasColumnType("float");

                    b.Property<double>("C12_C16")
                        .HasColumnType("float");

                    b.Property<double>("C16_C21")
                        .HasColumnType("float");

                    b.Property<double>("C17_Pristano")
                        .HasColumnType("float");

                    b.Property<double>("C18_Fitano")
                        .HasColumnType("float");

                    b.Property<double>("C21_C35")
                        .HasColumnType("float");

                    b.Property<double>("C6_C8")
                        .HasColumnType("float");

                    b.Property<double>("C8_C10")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("Dro")
                        .HasColumnType("float");

                    b.Property<double>("DynamicViscosity")
                        .HasColumnType("float");

                    b.Property<double>("Ethylbenzene")
                        .HasColumnType("float");

                    b.Property<double>("Gro")
                        .HasColumnType("float");

                    b.Property<double>("Mro")
                        .HasColumnType("float");

                    b.Property<double>("RealDensity")
                        .HasColumnType("float");

                    b.Property<double>("Tolueno")
                        .HasColumnType("float");

                    b.Property<string>("WellId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Xylenes")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("WellId");

                    b.ToTable("FlnaAnalyses");
                });

            modelBuilder.Entity("Wells.Model.Measurement", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("FlnaDepth")
                        .HasColumnType("float");

                    b.Property<double>("WaterDepth")
                        .HasColumnType("float");

                    b.Property<string>("WellId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("WellId");

                    b.ToTable("Measurements");
                });

            modelBuilder.Entity("Wells.Model.Precipitation", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("Millimeters")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Precipitations");
                });

            modelBuilder.Entity("Wells.Model.SoilAnalysis", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Acenafthene")
                        .HasColumnType("float");

                    b.Property<double>("Acenaphthylene")
                        .HasColumnType("float");

                    b.Property<double>("Anthracene")
                        .HasColumnType("float");

                    b.Property<double>("Arsenic")
                        .HasColumnType("float");

                    b.Property<double>("Benzene")
                        .HasColumnType("float");

                    b.Property<double>("BenzoAAnthracene")
                        .HasColumnType("float");

                    b.Property<double>("BenzoAPyrene")
                        .HasColumnType("float");

                    b.Property<double>("BenzoBFluoranthene")
                        .HasColumnType("float");

                    b.Property<double>("BenzoGhiPerylene")
                        .HasColumnType("float");

                    b.Property<double>("BenzoKFluoranthene")
                        .HasColumnType("float");

                    b.Property<double>("C10_C12Aliphatic")
                        .HasColumnType("float");

                    b.Property<double>("C10_C12Aromatic")
                        .HasColumnType("float");

                    b.Property<double>("C12_C16Aliphatic")
                        .HasColumnType("float");

                    b.Property<double>("C12_C16Aromatic")
                        .HasColumnType("float");

                    b.Property<double>("C16_C21Aliphatic")
                        .HasColumnType("float");

                    b.Property<double>("C16_C21Aromatic")
                        .HasColumnType("float");

                    b.Property<double>("C21_C35Aliphatic")
                        .HasColumnType("float");

                    b.Property<double>("C21_C35Aromatic")
                        .HasColumnType("float");

                    b.Property<double>("C6_C8Aliphatic")
                        .HasColumnType("float");

                    b.Property<double>("C6_C8Aromatic")
                        .HasColumnType("float");

                    b.Property<double>("C8_C10Aliphatic")
                        .HasColumnType("float");

                    b.Property<double>("C8_C10Aromatic")
                        .HasColumnType("float");

                    b.Property<double>("Cadmium")
                        .HasColumnType("float");

                    b.Property<double>("Copper")
                        .HasColumnType("float");

                    b.Property<double>("Crysene")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("DibenzoAhAnthracene")
                        .HasColumnType("float");

                    b.Property<double>("Dro")
                        .HasColumnType("float");

                    b.Property<double>("Ethylbenzene")
                        .HasColumnType("float");

                    b.Property<double>("Fenanthrene")
                        .HasColumnType("float");

                    b.Property<double>("Fluoranthene")
                        .HasColumnType("float");

                    b.Property<double>("Fluorene")
                        .HasColumnType("float");

                    b.Property<double>("Gro")
                        .HasColumnType("float");

                    b.Property<double>("Humidity")
                        .HasColumnType("float");

                    b.Property<double>("Indeno123CdPyrene")
                        .HasColumnType("float");

                    b.Property<double>("Lead")
                        .HasColumnType("float");

                    b.Property<double>("Mercury")
                        .HasColumnType("float");

                    b.Property<double>("Mro")
                        .HasColumnType("float");

                    b.Property<double>("Naphthalene")
                        .HasColumnType("float");

                    b.Property<double>("Nickel")
                        .HasColumnType("float");

                    b.Property<double>("OilsAndFats")
                        .HasColumnType("float");

                    b.Property<double>("Ph")
                        .HasColumnType("float");

                    b.Property<double>("Pyrene")
                        .HasColumnType("float");

                    b.Property<double>("Selenium")
                        .HasColumnType("float");

                    b.Property<double>("Tolueno")
                        .HasColumnType("float");

                    b.Property<double>("TotalChrome")
                        .HasColumnType("float");

                    b.Property<double>("TotalHydrocarbonsEpa8015")
                        .HasColumnType("float");

                    b.Property<double>("TotalHydrocarbonsTnrcc1005")
                        .HasColumnType("float");

                    b.Property<double>("TotalXylene")
                        .HasColumnType("float");

                    b.Property<string>("WellId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("XyleneO")
                        .HasColumnType("float");

                    b.Property<double>("XylenePm")
                        .HasColumnType("float");

                    b.Property<double>("Zinc")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("WellId");

                    b.ToTable("SoilAnalyses");
                });

            modelBuilder.Entity("Wells.Model.WaterAnalysis", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Acenafthene")
                        .HasColumnType("float");

                    b.Property<double>("Acenaphthylene")
                        .HasColumnType("float");

                    b.Property<double>("Anthracene")
                        .HasColumnType("float");

                    b.Property<double>("Arsenic")
                        .HasColumnType("float");

                    b.Property<double>("Benzene")
                        .HasColumnType("float");

                    b.Property<double>("BenzoAAnthracene")
                        .HasColumnType("float");

                    b.Property<double>("BenzoAPyrene")
                        .HasColumnType("float");

                    b.Property<double>("BenzoBFluoranthene")
                        .HasColumnType("float");

                    b.Property<double>("BenzoGhiPerylene")
                        .HasColumnType("float");

                    b.Property<double>("BenzoKFluoranthene")
                        .HasColumnType("float");

                    b.Property<double>("BicarbonateAlkalinity")
                        .HasColumnType("float");

                    b.Property<double>("Cadmium")
                        .HasColumnType("float");

                    b.Property<double>("Calcium")
                        .HasColumnType("float");

                    b.Property<double>("CarbonateAlkalinity")
                        .HasColumnType("float");

                    b.Property<double>("Chlorides")
                        .HasColumnType("float");

                    b.Property<double>("Cobalt")
                        .HasColumnType("float");

                    b.Property<double>("Conductivity")
                        .HasColumnType("float");

                    b.Property<double>("Copper")
                        .HasColumnType("float");

                    b.Property<double>("Crysene")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("DibenzoAhAnthracene")
                        .HasColumnType("float");

                    b.Property<double>("Dro")
                        .HasColumnType("float");

                    b.Property<double>("DryWaste")
                        .HasColumnType("float");

                    b.Property<double>("Ethylbenzene")
                        .HasColumnType("float");

                    b.Property<double>("Fenanthrene")
                        .HasColumnType("float");

                    b.Property<double>("Fluoranthene")
                        .HasColumnType("float");

                    b.Property<double>("Fluorene")
                        .HasColumnType("float");

                    b.Property<double>("Fluorides")
                        .HasColumnType("float");

                    b.Property<double>("Gro")
                        .HasColumnType("float");

                    b.Property<double>("Indeno123CdPyrene")
                        .HasColumnType("float");

                    b.Property<double>("Lead")
                        .HasColumnType("float");

                    b.Property<double>("Magnesium")
                        .HasColumnType("float");

                    b.Property<double>("Mercury")
                        .HasColumnType("float");

                    b.Property<double>("Mro")
                        .HasColumnType("float");

                    b.Property<double>("Naphthalene")
                        .HasColumnType("float");

                    b.Property<double>("Nickel")
                        .HasColumnType("float");

                    b.Property<double>("Nitrates")
                        .HasColumnType("float");

                    b.Property<double>("Ph")
                        .HasColumnType("float");

                    b.Property<double>("Potassium")
                        .HasColumnType("float");

                    b.Property<double>("Pyrene")
                        .HasColumnType("float");

                    b.Property<double>("Sodium")
                        .HasColumnType("float");

                    b.Property<double>("Sulfates")
                        .HasColumnType("float");

                    b.Property<double>("Tolueno")
                        .HasColumnType("float");

                    b.Property<double>("TotalChrome")
                        .HasColumnType("float");

                    b.Property<double>("TotalHydrocarbonsEpa8015")
                        .HasColumnType("float");

                    b.Property<double>("TotalHydrocarbonsTnrcc1005")
                        .HasColumnType("float");

                    b.Property<double>("TotalSulfur")
                        .HasColumnType("float");

                    b.Property<double>("TotalXylene")
                        .HasColumnType("float");

                    b.Property<string>("WellId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("XyleneO")
                        .HasColumnType("float");

                    b.Property<double>("XylenePm")
                        .HasColumnType("float");

                    b.Property<double>("Zinc")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("WellId");

                    b.ToTable("WaterAnalyses");
                });

            modelBuilder.Entity("Wells.Model.Well", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Bottom")
                        .HasColumnType("float");

                    b.Property<bool>("Exists")
                        .HasColumnType("bit");

                    b.Property<double>("Height")
                        .HasColumnType("float");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WellType")
                        .HasColumnType("int");

                    b.Property<double>("X")
                        .HasColumnType("float");

                    b.Property<double>("Y")
                        .HasColumnType("float");

                    b.Property<double>("Z")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Wells");
                });

            modelBuilder.Entity("Wells.Model.ExternalFile", b =>
                {
                    b.HasOne("Wells.Model.Well", "Well")
                        .WithMany("Files")
                        .HasForeignKey("WellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wells.Model.FlnaAnalysis", b =>
                {
                    b.HasOne("Wells.Model.Well", "Well")
                        .WithMany("FlnaAnalyses")
                        .HasForeignKey("WellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wells.Model.Measurement", b =>
                {
                    b.HasOne("Wells.Model.Well", "Well")
                        .WithMany("Measurements")
                        .HasForeignKey("WellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wells.Model.SoilAnalysis", b =>
                {
                    b.HasOne("Wells.Model.Well", "Well")
                        .WithMany("SoilAnalyses")
                        .HasForeignKey("WellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wells.Model.WaterAnalysis", b =>
                {
                    b.HasOne("Wells.Model.Well", "Well")
                        .WithMany("WaterAnalyses")
                        .HasForeignKey("WellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
