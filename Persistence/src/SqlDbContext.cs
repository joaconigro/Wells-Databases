using Microsoft.EntityFrameworkCore;

namespace Wells.Persistence
{
    public class SqlDbContext : ApplicationDbContext
    {
        #region Constructor
        public SqlDbContext(string connectionString) : base(connectionString)
        {
        }

        public SqlDbContext() { }
        #endregion Constructor

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(connectionString);
            }
        }
    }
}
