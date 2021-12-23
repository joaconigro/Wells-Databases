using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Wells.Persistence.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApplicationDbContext Context;
        private PrecipitationsRepository precipitations;
        private ExternalFilesRepository externalFiles;
        private MeasurementsRepository measurements;
        private SoilAnalysesRepository soilAnalyses;
        private WaterAnalysesRepository waterAnalyses;
        private FlnaAnalysesRepository flnaAnalyses;
        private WellsRepository wells;

        public PrecipitationsRepository Precipitations
        {
            get
            {
                if (precipitations == null) { precipitations = new PrecipitationsRepository(Context); }
                return precipitations;
            }
        }

        public ExternalFilesRepository ExternalFiles
        {
            get
            {
                if (externalFiles == null) { externalFiles = new ExternalFilesRepository(Context); }
                return externalFiles;
            }
        }

        public MeasurementsRepository Measurements
        {
            get
            {
                if (measurements == null) { measurements = new MeasurementsRepository(Context); }
                return measurements;
            }
        }

        public SoilAnalysesRepository SoilAnalyses
        {
            get
            {
                if (soilAnalyses == null) { soilAnalyses = new SoilAnalysesRepository(Context); }
                return soilAnalyses;
            }
        }

        public WaterAnalysesRepository WaterAnalyses
        {
            get
            {
                if (waterAnalyses == null) { waterAnalyses = new WaterAnalysesRepository(Context); }
                return waterAnalyses;
            }
        }

        public FlnaAnalysesRepository FlnaAnalyses
        {
            get
            {
                if (flnaAnalyses == null) { flnaAnalyses = new FlnaAnalysesRepository(Context); }
                return flnaAnalyses;
            }
        }

        public WellsRepository Wells
        {
            get
            {
                if (wells == null) { wells = new WellsRepository(Context); }
                return wells;
            }
        }

        public static bool IsInstatiated => Instance != null;

        public static RepositoryWrapper Instance { get; private set; }

        RepositoryWrapper(ApplicationDbContext repositoryContext)
        {
            Context = repositoryContext;
        }

        public static RepositoryWrapper Instantiate(string connectionString, string dbName)
        {
            var sqlDbContext = new SqlDbContext(connectionString);
            var sqlExists = sqlDbContext.Database.CanConnect();
            

            var sqliteContext = new SqliteDbContext(dbName);
            if (!File.Exists(sqliteContext.DbFilename))
            {
                sqliteContext.Database.Migrate();
                Instance = new RepositoryWrapper(sqliteContext);
                if (sqlExists)
                {
                    sqliteContext.Precipitations.AddRange(sqlDbContext.Precipitations);
                    sqliteContext.Files.AddRange(sqlDbContext.Files);
                    sqliteContext.WaterAnalyses.AddRange(sqlDbContext.WaterAnalyses);
                    sqliteContext.SoilAnalyses.AddRange(sqlDbContext.SoilAnalyses);
                    sqliteContext.FlnaAnalyses.AddRange(sqlDbContext.FlnaAnalyses);
                    sqliteContext.Measurements.AddRange(sqlDbContext.Measurements);
                    sqliteContext.Wells.AddRange(sqlDbContext.Wells);

                    sqlDbContext.Database.EnsureDeleted();
                }

                Instance.SaveChanges();
            }

            if (Instance == null)
            {
                throw new ArgumentNullException("El repositorio no pudo ser instanciado.");
            }

            return Instance;
        }

        public async Task DropSchema(string connectionString, string dbName)
        {
            await Context.Database.EnsureDeletedAsync();
            Instantiate(connectionString, dbName);
            Precipitations?.RaiseEntitiesRemoved();
            ExternalFiles?.RaiseEntitiesRemoved();
            WaterAnalyses?.RaiseEntitiesRemoved();
            FlnaAnalyses?.RaiseEntitiesRemoved();
            SoilAnalyses?.RaiseEntitiesRemoved();
            Measurements?.RaiseEntitiesRemoved();
            Wells?.RaiseEntitiesRemoved();
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
