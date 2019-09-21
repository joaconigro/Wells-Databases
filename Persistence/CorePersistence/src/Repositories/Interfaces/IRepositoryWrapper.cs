namespace Wells.CorePersistence.Repositories
{
    public interface IRepositoryWrapper
    {
        PrecipitationsRepository Precipitations { get; }
        ExternalFilesRepository ExternalFiles { get; }
        MeasurementsRepository Measurements { get; }
        SoilAnalysesRepository SoilAnalyses { get; }
        FLNAAnalysesRepository FLNAAnalyses { get; }
        WaterAnalysesRepository WaterAnalyses { get; }
        WellsRepository Wells { get; }

        void SaveChanges();
    }
}
