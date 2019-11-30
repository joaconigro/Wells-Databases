using Microsoft.EntityFrameworkCore;
using Wells.Model;

namespace Wells.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Pozos;Integrated Security=True;MultipleActiveResultSets=True";

        #region Constructor
        public ApplicationDbContext(DbContextOptions options) :
        base(options)
        {
        }

        public ApplicationDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ApplicationDbContext() { }
        #endregion Constructor


        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Well>().ToTable("Wells");
            modelBuilder.Entity<Well>().HasMany(w => w.Measurements).WithOne(i => i.Well).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Well>().HasMany(w => w.WaterAnalyses).WithOne(i => i.Well).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Well>().HasMany(w => w.Files).WithOne(i => i.Well).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Measurement>().ToTable("Measurements");
            modelBuilder.Entity<Measurement>().HasOne(u => u.Well).WithMany(i => i.Measurements).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WaterAnalysis>().ToTable("WaterAnalyses");
            modelBuilder.Entity<WaterAnalysis>().HasOne(u => u.Well).WithMany(c => c.WaterAnalyses).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExternalFile>().ToTable("ExternalFiles");
            modelBuilder.Entity<ExternalFile>().HasOne(u => u.Well).WithMany(c => c.Files).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Precipitation>().ToTable("Precipitations");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(connectionString);
            }
        }


        public static DbContextOptions GetSqlOptions(string connectionString)
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
        public DbSet<WaterAnalysis> WaterAnalyses { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<ExternalFile> Files { get; set; }
        public DbSet<Precipitation> Precipitations { get; set; }
        #endregion Properties
    }
}
