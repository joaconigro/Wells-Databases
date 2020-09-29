using System.Linq;
using Wells.Model;

namespace Wells.Persistence.Repositories
{
    public class WaterAnalysesRepository : RepositoryBase<WaterAnalysis>, IBussinessObjectRepository
    {
        public WaterAnalysesRepository(ApplicationDbContext context) : base(context) { }

        public override IQueryable<WaterAnalysis> All => Context.WaterAnalyses.ToList()
            .OrderBy(p => p.WellName)
            .ThenBy(p => p.Date)
            .AsQueryable();

        public override bool ContainsName(string name)
        {
            return true;
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
            return null;
        }
    }
}
