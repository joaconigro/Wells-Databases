using Wells.Model;

namespace Wells.Persistence.Repositories
{
    public class FlnaAnalysesRepository : RepositoryBase<FlnaAnalysis>, IBussinessObjectRepository
    {
        public FlnaAnalysesRepository(ApplicationDbContext context) : base(context) { }

        public override bool ContainsName(string name)
        {
            return true;
        }

        public override bool Exists(string id)
        {
            return Exists((c) => c.Id == id);
        }

        public override FlnaAnalysis Find(string id)
        {
            return Find((c) => c.Id == id);
        }

        public override FlnaAnalysis FindByName(string name)
        {
            return null;
        }       
    }
}
