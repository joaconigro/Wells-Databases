using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Wells.Model;

namespace Wells.Persistence.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApplicationDbContext Context;
        private PrecipitationsRepository precipitations;
        private ExternalFilesRepository externalFiles;
        private MeasurementsRepository measurements;
        private WaterAnalysesRepository waterAnalyses;
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
        
        public WaterAnalysesRepository WaterAnalyses
        {
            get
            {
                if (waterAnalyses == null) { waterAnalyses = new WaterAnalysesRepository(Context); }
                return waterAnalyses;
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

        public static RepositoryWrapper Instantiate(string connectionString)
        {
            var context = new ApplicationDbContext(connectionString);
            context.Database.Migrate();
            Instance = new RepositoryWrapper(context);
            if (Instance == null)
            {
                throw new ArgumentNullException("El repositorio se pudo ser instanciado.");
            }
            return Instance;
        }

        public async Task DropSchema(string connectionString)
        {
            await Context.Database.EnsureDeletedAsync();
            Instantiate(connectionString);
            Precipitations?.RaiseEntitiesRemoved();
            ExternalFiles?.RaiseEntitiesRemoved();
            WaterAnalyses?.RaiseEntitiesRemoved();
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
