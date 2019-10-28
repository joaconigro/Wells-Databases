using Wells.Model;

namespace Wells.Persistence.Repositories
{
    public class WaterAnalysesRepository : RepositoryBase<WaterAnalysis>, IBussinessObjectRepository
    {
        public WaterAnalysesRepository(ApplicationDbContext context) : base(context) { }

        public override bool ContainsName(string name)
        {
            return true; //Exists((c) => c.Name == name);
        }

        public override bool Exists(string id)
        {
            return Exists((c) => c.Id == id);
        }

        public override WaterAnalysis Find(string id)
        {
            return Find((c) => c.Id == id);
        }

        public override WaterAnalysis FindByName(string name)
        {
            return null; //Find((c) => c.Name == name);
        }       
    }
}
