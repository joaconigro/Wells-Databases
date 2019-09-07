using System.Collections.Generic;
using System.Linq;
using Wells.YPFModel;

namespace Wells.CorePersistence.Repositories
{
    public class WellsRepository : RepositoryBase<Well>, IBussinessObjectRepository
    {
        public IEnumerable<string> Names => All.Select(w => w.Name);
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
            if (string.IsNullOrEmpty(entity.Name)) {
                return RejectedReasons.WellNameEmpty;
            } else if (Exists(entity.Id)) {
                return RejectedReasons.DuplicatedId;
            }
            else if (ContainsName(entity.Name)) {
                return RejectedReasons.DuplicatedName;
            }
            return RejectedReasons.None;
        }
    }
}
