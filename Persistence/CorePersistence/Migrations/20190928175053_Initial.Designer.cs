﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wells.CorePersistence;

namespace Wells.CorePersistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190928175053_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Wells.YPFModel.ExternalFile", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Data")
                        .IsRequired();

                    b.Property<string>("FileExtension")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.Property<string>("WellId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("WellId");

                    b.ToTable("ExternalFiles");
                });

            modelBuilder.Entity("Wells.YPFModel.FLNAAnalysis", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Benzene");

                    b.Property<double>("C10_C12");

                    b.Property<double>("C12_C16");

                    b.Property<double>("C16_C21");

                    b.Property<double>("C17_Pristano");

                    b.Property<double>("C18_Fitano");

                    b.Property<double>("C21_C35");

                    b.Property<double>("C6_C8");

                    b.Property<double>("C8_C10");

                    b.Property<double>("DRO");

                    b.Property<DateTime>("Date");

                    b.Property<double>("DynamicViscosity");

                    b.Property<double>("Ethylbenzene");

                    b.Property<double>("GRO");

                    b.Property<double>("MRO");

                    b.Property<double>("RealDensity");

                    b.Property<double>("Tolueno");

                    b.Property<string>("WellId")
                        .IsRequired();

                    b.Property<double>("Xylenes");

                    b.HasKey("Id");

                    b.HasIndex("WellId");

                    b.ToTable("FLNAAnalyses");
                });

            modelBuilder.Entity("Wells.YPFModel.Measurement", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<DateTime>("Date");

                    b.Property<double>("FLNADepth");

                    b.Property<double>("WaterDepth");

                    b.Property<string>("WellId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("WellId");

                    b.ToTable("Measurements");
                });

            modelBuilder.Entity("Wells.YPFModel.Precipitation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Millimeters");

                    b.Property<DateTime>("PrecipitationDate");

                    b.HasKey("Id");

                    b.ToTable("Precipitations");
                });

            modelBuilder.Entity("Wells.YPFModel.SoilAnalysis", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Acenafthene");

                    b.Property<double>("Acenaphthylene");

                    b.Property<double>("Anthracene");

                    b.Property<double>("Arsenic");

                    b.Property<double>("Benzene");

                    b.Property<double>("BenzoAAnthracene");

                    b.Property<double>("BenzoAPyrene");

                    b.Property<double>("BenzoBFluoranthene");

                    b.Property<double>("BenzoGHIPerylene");

                    b.Property<double>("BenzoKFluoranthene");

                    b.Property<double>("C10_C12Aliphatic");

                    b.Property<double>("C10_C12Aromatic");

                    b.Property<double>("C12_C16Aliphatic");

                    b.Property<double>("C12_C16Aromatic");

                    b.Property<double>("C16_C21Aliphatic");

                    b.Property<double>("C16_C21Aromatic");

                    b.Property<double>("C21_C35Aliphatic");

                    b.Property<double>("C21_C35Aromatic");

                    b.Property<double>("C6_C8Aliphatic");

                    b.Property<double>("C6_C8Aromatic");

                    b.Property<double>("C8_C10Aliphatic");

                    b.Property<double>("C8_C10Aromatic");

                    b.Property<double>("Cadmium");

                    b.Property<double>("Copper");

                    b.Property<double>("Crysene");

                    b.Property<double>("DRO");

                    b.Property<DateTime>("Date");

                    b.Property<double>("DibenzoAHAnthracene");

                    b.Property<double>("Ethylbenzene");

                    b.Property<double>("Fenanthrene");

                    b.Property<double>("Fluoranthene");

                    b.Property<double>("Fluorene");

                    b.Property<double>("GRO");

                    b.Property<double>("Humidity");

                    b.Property<double>("Indeno123CDPyrene");

                    b.Property<double>("Lead");

                    b.Property<double>("MRO");

                    b.Property<double>("Mercury");

                    b.Property<double>("Naphthalene");

                    b.Property<double>("Nickel");

                    b.Property<double>("OilsAndFats");

                    b.Property<double>("PH");

                    b.Property<double>("Pyrene");

                    b.Property<double>("Selenium");

                    b.Property<double>("Tolueno");

                    b.Property<double>("TotalChrome");

                    b.Property<double>("TotalHydrocarbons_EPA8015");

                    b.Property<double>("TotalHydrocarbons_TNRCC1005");

                    b.Property<double>("TotalXylene");

                    b.Property<string>("WellId")
                        .IsRequired();

                    b.Property<double>("XyleneO");

                    b.Property<double>("XylenePM");

                    b.Property<double>("Zinc");

                    b.HasKey("Id");

                    b.HasIndex("WellId");

                    b.ToTable("SoilAnalyses");
                });

            modelBuilder.Entity("Wells.YPFModel.WaterAnalysis", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Acenafthene");

                    b.Property<double>("Acenaphthylene");

                    b.Property<double>("Anthracene");

                    b.Property<double>("Arsenic");

                    b.Property<double>("Benzene");

                    b.Property<double>("BenzoAAnthracene");

                    b.Property<double>("BenzoAPyrene");

                    b.Property<double>("BenzoBFluoranthene");

                    b.Property<double>("BenzoGHIPerylene");

                    b.Property<double>("BenzoKFluoranthene");

                    b.Property<double>("BicarbonateAlkalinity");

                    b.Property<double>("Cadmium");

                    b.Property<double>("Calcium");

                    b.Property<double>("CarbonateAlkalinity");

                    b.Property<double>("Chlorides");

                    b.Property<double>("Cobalt");

                    b.Property<double>("Conductivity");

                    b.Property<double>("Copper");

                    b.Property<double>("Crysene");

                    b.Property<double>("DRO");

                    b.Property<DateTime>("Date");

                    b.Property<double>("DibenzoAHAnthracene");

                    b.Property<double>("DryWaste");

                    b.Property<double>("Ethylbenzene");

                    b.Property<double>("Fenanthrene");

                    b.Property<double>("Fluoranthene");

                    b.Property<double>("Fluorene");

                    b.Property<double>("Fluorides");

                    b.Property<double>("GRO");

                    b.Property<double>("Indeno123CDPyrene");

                    b.Property<double>("Lead");

                    b.Property<double>("MRO");

                    b.Property<double>("Magnesium");

                    b.Property<double>("Mercury");

                    b.Property<double>("Naphthalene");

                    b.Property<double>("Nickel");

                    b.Property<double>("Nitrates");

                    b.Property<double>("PH");

                    b.Property<double>("Potassium");

                    b.Property<double>("Pyrene");

                    b.Property<double>("Sodium");

                    b.Property<double>("Sulfates");

                    b.Property<double>("Tolueno");

                    b.Property<double>("TotalChrome");

                    b.Property<double>("TotalHydrocarbons_EPA8015");

                    b.Property<double>("TotalHydrocarbons_TNRCC1005");

                    b.Property<double>("TotalSulfur");

                    b.Property<double>("TotalXylene");

                    b.Property<string>("WellId")
                        .IsRequired();

                    b.Property<double>("XyleneO");

                    b.Property<double>("XylenePM");

                    b.Property<double>("Zinc");

                    b.HasKey("Id");

                    b.HasIndex("WellId");

                    b.ToTable("WaterAnalyses");
                });

            modelBuilder.Entity("Wells.YPFModel.Well", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Bottom");

                    b.Property<bool>("Exists");

                    b.Property<double>("Height");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Name");

                    b.Property<int>("WellType");

                    b.Property<double>("X");

                    b.Property<double>("Y");

                    b.Property<double>("Z");

                    b.HasKey("Id");

                    b.ToTable("Wells");
                });

            modelBuilder.Entity("Wells.YPFModel.ExternalFile", b =>
                {
                    b.HasOne("Wells.YPFModel.Well", "Well")
                        .WithMany("Files")
                        .HasForeignKey("WellId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wells.YPFModel.FLNAAnalysis", b =>
                {
                    b.HasOne("Wells.YPFModel.Well", "Well")
                        .WithMany("FLNAAnalyses")
                        .HasForeignKey("WellId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wells.YPFModel.Measurement", b =>
                {
                    b.HasOne("Wells.YPFModel.Well", "Well")
                        .WithMany("Measurements")
                        .HasForeignKey("WellId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wells.YPFModel.SoilAnalysis", b =>
                {
                    b.HasOne("Wells.YPFModel.Well", "Well")
                        .WithMany("SoilAnalyses")
                        .HasForeignKey("WellId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wells.YPFModel.WaterAnalysis", b =>
                {
                    b.HasOne("Wells.YPFModel.Well", "Well")
                        .WithMany("WaterAnalyses")
                        .HasForeignKey("WellId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}