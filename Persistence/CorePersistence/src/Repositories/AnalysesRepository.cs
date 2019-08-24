using Wells.YPFModel;
using System.Linq;

namespace Wells.CorePersistence.Repositories
{
    public class AnalysesRepository : RepositoryBase<ChemicalAnalysis>, IBussinessObjectRepository
    {
        public AnalysesRepository(ApplicationDbContext context) : base(context) { }

        public override bool ContainsName(string name)
        {
            return Context.Analyses.ToList().Exists((c) => c.Name == name);
        }

        public override bool Exists(string id)
        {
            return Context.Analyses.ToList().Exists((c) => c.Id == id);
        }

        public override ChemicalAnalysis Find(string id)
        {
            return Context.Analyses.ToList().FirstOrDefault((c) => c.Id == id);
        }

        public override ChemicalAnalysis FindByName(string name)
        {
            return Context.Analyses.ToList().FirstOrDefault((c) => c.Name == name);
        }
    }
}
