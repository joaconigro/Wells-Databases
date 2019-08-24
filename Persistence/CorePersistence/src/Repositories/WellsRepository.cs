using System.Linq;
using Wells.YPFModel;

namespace Wells.CorePersistence.Repositories
{
    public class WellsRepository : RepositoryBase<Well>, IBussinessObjectRepository
    {
        public WellsRepository(ApplicationDbContext context) : base(context) { }

        public override bool ContainsName(string name)
        {
            return Context.Wells.ToList().Exists((c) => c.Name == name);
        }

        public override bool Exists(string id)
        {
            return Context.Wells.ToList().Exists((c) => c.Id == id);
        }

        public override Well Find(string id)
        {
            return Context.Wells.ToList().FirstOrDefault((c) => c.Id == id);
        }

        public override Well FindByName(string name)
        {
            return Context.Wells.ToList().FirstOrDefault((c) => c.Name == name);
        }

        protected override RejectedReasons OnAddingOrUpdating(Well entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
