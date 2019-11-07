namespace Wells.Persistence.Repositories
{
    public interface IRepositoryWrapper
    {
        PrecipitationsRepository Precipitations { get; }
        ExternalFilesRepository ExternalFiles { get; }
        MeasurementsRepository Measurements { get; }
        SoilAnalysesRepository SoilAnalyses { get; }
        FlnaAnalysesRepository FlnaAnalyses { get; }
        WaterAnalysesRepository WaterAnalyses { get; }
        WellsRepository Wells { get; }

        void SaveChanges();
    }
}
