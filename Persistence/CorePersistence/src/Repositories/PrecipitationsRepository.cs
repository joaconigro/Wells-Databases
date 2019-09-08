using System.Linq;
using Wells.YPFModel;

namespace Wells.CorePersistence.Repositories
{
    public class PrecipitationsRepository : RepositoryBase<Precipitation>, IBussinessObjectRepository
    {
        public PrecipitationsRepository(ApplicationDbContext context) : base(context) { }

        public override bool ContainsName(string name)
        {
            return Exists((c) => c.Name == name);
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
            return Find((c) => c.Name == name);
        }

        //protected override RejectedReasons OnAddingOrUpdating(Precipitation entity)
        //{
        //    if (Exists(entity.Id))
        //    {
        //        return RejectedReasons.DuplicatedId;
        //    }
        //    else if (Exists((p) => p.PrecipitationDate == entity.PrecipitationDate))
        //    {
        //        return RejectedReasons.DuplicatedDate;
        //    }
        //    return RejectedReasons.None;
        //}
    }
}
