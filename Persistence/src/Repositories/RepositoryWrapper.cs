using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Wells.Model;

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
        private FLNAAnalysesRepository fLNAAnalyses;
        private WellsRepository wells;

        private static RepositoryWrapper instance;
        

        public PrecipitationsRepository Precipitations
        {
            get
            {
                if (precipitations == null)
                    precipitations = new PrecipitationsRepository(Context);
                return precipitations;
            }
        }

        public ExternalFilesRepository ExternalFiles
        {
            get
            {
                if (externalFiles == null)
                    externalFiles = new ExternalFilesRepository(Context);
                return externalFiles;
            }
        }

        public MeasurementsRepository Measurements
        {
            get
            {
                if (measurements == null)
                    measurements = new MeasurementsRepository(Context);
                return measurements;
            }
        }

        public SoilAnalysesRepository SoilAnalyses
        {
            get
            {
                if (soilAnalyses == null)
                    soilAnalyses = new SoilAnalysesRepository(Context);
                return soilAnalyses;
            }
        }

        public WaterAnalysesRepository WaterAnalyses
        {
            get
            {
                if (waterAnalyses == null)
                    waterAnalyses = new WaterAnalysesRepository(Context);
                return waterAnalyses;
            }
        }

        public FLNAAnalysesRepository FLNAAnalyses
        {
            get
            {
                if (fLNAAnalyses == null)
                    fLNAAnalyses = new FLNAAnalysesRepository(Context);
                return fLNAAnalyses;
            }
        }

        public WellsRepository Wells
        {
            get
            {
                if (wells == null)
                    wells = new WellsRepository(Context);
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
            else if (typeof(T) == typeof(FLNAAnalysis))
            {
                return FLNAAnalyses;
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

        public static bool IsInstatiated => instance != null;

        public static RepositoryWrapper Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new NullReferenceException("El repositorio no fue instanciado. Use el método 'Instantiate' para obtener un singleton de RepositoryWrapper");
                }
                return instance;
            }
        }

        RepositoryWrapper(ApplicationDbContext repositoryContext)
        {
            Context = repositoryContext;
        }

        public static void Instantiate(string connectionString)
        {
            var context = new ApplicationDbContext(connectionString);
            context.Database.Migrate();
            instance = new RepositoryWrapper(context);
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
