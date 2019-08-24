using System.Linq;
using Wells.YPFModel;

namespace Wells.CorePersistence.Repositories
{
    public class PrecipitationsRepository : RepositoryBase<Precipitation>, IBussinessObjectRepository
    {
        public PrecipitationsRepository(ApplicationDbContext context) : base(context) { }

        public override bool ContainsName(string name)
        {
            return Context.Precipitations.ToList().Exists((c) => c.Name == name);
        }

        public override bool Exists(string id)
        {
            return Context.Precipitations.ToList().Exists((c) => c.Id == id);
        }

        public override Precipitation Find(string id)
        {
            return Context.Precipitations.ToList().FirstOrDefault((c) => c.Id == id);
        }

        public override Precipitation FindByName(string name)
        {
            return Context.Precipitations.ToList().FirstOrDefault((c) => c.Name == name);
        }
    }
}
