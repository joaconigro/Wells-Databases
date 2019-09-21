﻿using Microsoft.EntityFrameworkCore;
using Wells.YPFModel;

namespace Wells.CorePersistence
{
    public class ApplicationDbContext : DbContext
    {
        #region Constructor
        public ApplicationDbContext(DbContextOptions options) :
        base(options)
        {
            //Database.Migrate();
        }

        public ApplicationDbContext() : base() { }

        public static string ConnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=WellsDB.mdf;Integrated Security=True";
        //private readonly bool useLazyLoading;

        //protected ApplicationDbContext(string connectionString, bool useLazyLoading) : base()
        //{
        //    this.connectionString = connectionString;
        //    this.useLazyLoading = useLazyLoading;
        //}
        #endregion Constructor


        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Well>().ToTable("Wells");
            modelBuilder.Entity<Well>().HasMany(w => w.Measurements).WithOne(i => i.Well);
            modelBuilder.Entity<Well>().HasMany(w => w.SoilAnalyses).WithOne(i => i.Well);
            modelBuilder.Entity<Well>().HasMany(w => w.FLNAAnalyses).WithOne(i => i.Well);
            modelBuilder.Entity<Well>().HasMany(w => w.WaterAnalyses).WithOne(i => i.Well);
            modelBuilder.Entity<Well>().HasMany(w => w.Files).WithOne(i => i.Well);

            modelBuilder.Entity<Measurement>().ToTable("Measurements");
            modelBuilder.Entity<Measurement>().HasOne(u => u.Well).WithMany(i => i.Measurements);

            modelBuilder.Entity<SoilAnalysis>().ToTable("SoilAnalyses");
            modelBuilder.Entity<SoilAnalysis>().HasOne(u => u.Well).WithMany(c => c.SoilAnalyses);

            modelBuilder.Entity<WaterAnalysis>().ToTable("WaterAnalyses");
            modelBuilder.Entity<WaterAnalysis>().HasOne(u => u.Well).WithMany(c => c.WaterAnalyses);

            modelBuilder.Entity<FLNAAnalysis>().ToTable("FLNAAnalyses");
            modelBuilder.Entity<FLNAAnalysis>().HasOne(u => u.Well).WithMany(c => c.FLNAAnalyses);

            modelBuilder.Entity<ExternalFile>().ToTable("ExternalFiles");
            modelBuilder.Entity<ExternalFile>().HasOne(u => u.Well).WithMany(c => c.Files);

            modelBuilder.Entity<Precipitation>().ToTable("Precipitations");

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(ConnectionString);
            }
        }


        public static DbContextOptions GetSQLOptions(string connectionString)
        {
            var builder = new DbContextOptionsBuilder();
            builder
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString);

            return builder.Options;
        }
        #endregion Methods


        #region Properties
        public DbSet<Well> Wells { get; set; }
        public DbSet<SoilAnalysis> SoilAnalyses { get; set; }
        public DbSet<WaterAnalysis> WaterAnalyses { get; set; }
        public DbSet<FLNAAnalysis> FLNAAnalyses { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<ExternalFile> Files { get; set; }
        public DbSet<Precipitation> Precipitations { get; set; }
        #endregion Properties
    }
}
