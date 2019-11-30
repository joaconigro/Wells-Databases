namespace Wells.Persistence.Repositories
{
    public interface IRepositoryWrapper
    {
        PrecipitationsRepository Precipitations { get; }
        ExternalFilesRepository ExternalFiles { get; }
        MeasurementsRepository Measurements { get; }
        WaterAnalysesRepository WaterAnalyses { get; }
        WellsRepository Wells { get; }

        void SaveChanges();
    }
}
