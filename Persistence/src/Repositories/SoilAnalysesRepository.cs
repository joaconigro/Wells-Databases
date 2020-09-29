using System.Linq;
using Wells.Model;

namespace Wells.Persistence.Repositories
{
    public class SoilAnalysesRepository : RepositoryBase<SoilAnalysis>, IBussinessObjectRepository
    {
        public SoilAnalysesRepository(ApplicationDbContext context) : base(context) { }

        public override IQueryable<SoilAnalysis> All => Context.SoilAnalyses.ToList()
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

        public override SoilAnalysis Find(string id)
        {
            return Find((c) => c.Id == id);
        }

        public override SoilAnalysis FindByName(string name)
        {
            return null;
        }       
    }
}
