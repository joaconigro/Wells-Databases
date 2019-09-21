using System.Linq;
using Wells.YPFModel;

namespace Wells.CorePersistence.Repositories
{
    public class PrecipitationsRepository : RepositoryBase<Precipitation>, IBussinessObjectRepository
    {
        public PrecipitationsRepository(ApplicationDbContext context) : base(context) { }

        public override bool ContainsName(string name)
        {
            return true; // Exists((c) => c.Name == name);
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
            return null; // Find((c) => c.Name == name);
        }

      
    }
}
