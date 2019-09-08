using System.Collections.Generic;
using System.Linq;
using Wells.YPFModel;

namespace Wells.CorePersistence.Repositories
{
    public class WellsRepository : RepositoryBase<Well>, IBussinessObjectRepository
    {
        public List<string> Names => Wells.Keys.ToList();

        private Dictionary<string, Well> _Wells;

        public Dictionary<string, Well> Wells
        {
            get
            {
                if (_Wells == null)
                {
                    _Wells = new Dictionary<string, Well>(All.ToDictionary(w => w.Name));
                }
                return _Wells;
            }
        }
        public WellsRepository(ApplicationDbContext context) : base(context) { }

        public override bool ContainsName(string name)
        {
            return Wells.ContainsKey(name);
        }

        public override bool Exists(string id)
        {
            return Exists((c) => c.Id == id);
        }

        public override Well Find(string id)
        {
            return Find((c) => c.Id == id);
        }

        public override Well FindByName(string name)
        {
            return Wells[name];
        }

        protected override void OnAddingOrUpdating()
        {
            _Wells = null;
            //if (string.IsNullOrEmpty(entity.Name)) {
            //    return RejectedReasons.WellNameEmpty;
            //} else if (Exists(entity.Id)) {
            //    return RejectedReasons.DuplicatedId;
            //}
            //else if (ContainsName(entity.Name)) {
            //    return RejectedReasons.DuplicatedName;
            //}
            //return RejectedReasons.None;
        }
    }
}
