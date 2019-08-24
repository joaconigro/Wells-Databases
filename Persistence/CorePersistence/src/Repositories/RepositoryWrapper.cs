using Microsoft.EntityFrameworkCore;
using System;
using Wells.YPFModel;

namespace Wells.CorePersistence.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ApplicationDbContext Context;
        private PrecipitationsRepository precipitations;
        private ExternalFilesRepository externalFiles;
        private MeasurementsRepository measurements;
        private AnalysesRepository analyses;
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

        public AnalysesRepository Analyses
        {
            get
            {
                if (analyses == null)
                    analyses = new AnalysesRepository(Context);
                return analyses;
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
            else if (typeof(T) == typeof(ChemicalAnalysis))
            {
                return Analyses;
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
            DbContextOptions options = ApplicationDbContext.GetSQLOptions(connectionString);
            var context = new ApplicationDbContext(options);
            instance = new RepositoryWrapper(context);
        }

        public static void Instantiate(ApplicationDbContext repositoryContext)
        {
            instance = new RepositoryWrapper(repositoryContext);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
