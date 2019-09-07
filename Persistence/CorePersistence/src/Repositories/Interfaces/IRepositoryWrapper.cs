namespace Wells.CorePersistence.Repositories
{
    public interface IRepositoryWrapper
    {
        PrecipitationsRepository Precipitations { get; }
        ExternalFilesRepository ExternalFiles { get; }
        MeasurementsRepository Measurements { get; }
        AnalysesRepository Analyses { get; }
        WellsRepository Wells { get; }

        void SaveChanges();
    }
}
