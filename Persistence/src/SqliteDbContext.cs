using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Wells.Persistence
{
    public class SqliteDbContext : ApplicationDbContext
    {

        #region Constructor
        public SqliteDbContext(string connectionString) : base(connectionString)
        {
            DbFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WellManager", $"{connectionString}.db");
            base.connectionString = $"DataSource={DbFilename}";
        }

        public SqliteDbContext() { }
        #endregion Constructor


        public string DbFilename { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlite(connectionString);
            }
        }
    }
}
