using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Wells.Persistence.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ApplicationDbContext Context;
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

        public static RepositoryWrapper Instantiate(string dbName)
        {
            var sqliteContext = new SqliteDbContext(dbName);
            sqliteContext.Database.Migrate();
            Instance = new RepositoryWrapper(sqliteContext); 

            if (Instance == null)
            {
                throw new ArgumentNullException("El repositorio no pudo ser instanciado.");
            }

            return Instance;
        }

        public async Task ResetSchema(string dbName)
        {
            await Context.Database.EnsureDeletedAsync();
            Instantiate(dbName);
            Precipitations?.RaiseEntitiesRemoved();
            ExternalFiles?.RaiseEntitiesRemoved();
            WaterAnalyses?.RaiseEntitiesRemoved();
            FlnaAnalyses?.RaiseEntitiesRemoved();
            SoilAnalyses?.RaiseEntitiesRemoved();
            Measurements?.RaiseEntitiesRemoved();
            Wells?.RaiseEntitiesRemoved();
        }

        public static void DropSchema(string dbName)
        {
            var sqliteContext = new SqliteDbContext(dbName);
            sqliteContext.Database.EnsureDeleted();
        }

        public async Task ChangeSchema(string dbName)
        {
            await SaveChangesAsync();
            await CloseConnection();

            Reinstantiate(dbName);
        }

        public void Reinstantiate(string dbName)
        {
            Instantiate(dbName);
            precipitations = new PrecipitationsRepository(Context);
            externalFiles = new ExternalFilesRepository(Context);
            waterAnalyses = new WaterAnalysesRepository(Context);
            flnaAnalyses = new FlnaAnalysesRepository(Context);
            soilAnalyses = new SoilAnalysesRepository(Context);
            measurements = new MeasurementsRepository(Context);
            wells = new WellsRepository(Context);
        }

        public async Task CloseConnection()
        {
            await Context.Database.CloseConnectionAsync();
            await Context.DisposeAsync();
            Context = null;
            GC.Collect();
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public void SaveDbToZip(string filename)
        {
            string dbFilename =  (Context as SqliteDbContext).DbFilename;
            string newFilename = $"{Path.GetFileName(dbFilename)}";

            var tempDbFile =  Path.Combine(Path.GetTempPath(), newFilename);

            File.Copy(dbFilename, tempDbFile, true);

            using ZipArchive archive = ZipFile.Open(filename, ZipArchiveMode.Create);
            archive.CreateEntryFromFile(tempDbFile, newFilename);

            if (File.Exists(tempDbFile))
            {
                File.Delete(tempDbFile);
            }
        }

        public static string ImportDbFromZip(string filename, string dbPath)
        {
            using ZipArchive archive = ZipFile.Open(filename, ZipArchiveMode.Read);
            var dbEntry = archive.Entries[0].FullName;
            var dbFilename = Path.Combine(dbPath, dbEntry);

            archive.ExtractToDirectory(dbPath, true);
            
            var dbName = Path.GetFileNameWithoutExtension(dbEntry);
            return dbName;
        }   
    }
}
