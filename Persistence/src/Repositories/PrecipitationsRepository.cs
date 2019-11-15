using System.Linq;
using Wells.Model;

namespace Wells.Persistence.Repositories
{
    public class PrecipitationsRepository : RepositoryBase<Precipitation>, IBussinessObjectRepository
    {
        public PrecipitationsRepository(ApplicationDbContext context) : base(context) { }

        public override IQueryable<Precipitation> All => Context.Precipitations.OrderBy(p => p.Date).AsQueryable();
        public override bool ContainsName(string name)
        {
            return true;
        }

        public override bool Exists(string id)
        {
            return Exists((c) => c.Id == id);
        }

        public override Precipitation Find(string id)
        {
            return Find((c) => c.Id == id);
        }

        public override Precipitation FindByName(string name)
        {
            return null;
        }

      
    }
}
