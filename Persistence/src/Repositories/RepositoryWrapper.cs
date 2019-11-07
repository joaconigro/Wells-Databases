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


        public IBussinessObjectRepository Repository<T>()
        {
            if (typeof(T) == typeof(Well))
            {
                return Wells;
            }
            else if (typeof(T) == typeof(SoilAnalysis))
            {
                return SoilAnalyses;
            }
            else if (typeof(T) == typeof(WaterAnalysis))
            {
                return WaterAnalyses;
            }
            else if (typeof(T) == typeof(FlnaAnalysis))
            {
                return FlnaAnalyses;
            }
            else if (typeof(T) == typeof(Measurement))
            {
                return Measurements;
            }
            else if (typeof(T) == typeof(Precipitation))
            {
                return Precipitations;
            }
            else if (typeof(T) == typeof(ExternalFile))
            {
                return ExternalFiles;
            }
            return null;
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
