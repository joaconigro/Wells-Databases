using System.Linq;
using Wells.YPFModel;

namespace Wells.CorePersistence.Repositories
{
    public class MeasurementsRepository : RepositoryBase<Measurement>, IBussinessObjectRepository
    {
        public MeasurementsRepository(ApplicationDbContext context) : base(context) { }

        public override bool ContainsName(string name)
        {
            return Exists((c) => c.Name == name);
        }

        public override bool Exists(string id)
        {
            return Exists((c) => c.Id == id);
        }

        public override Measurement Find(string id)
        {
            return Find((c) => c.Id == id);
        }

        public override Measurement FindByName(string name)
        {
            return Find((c) => c.Name == name);
        }

        //protected override RejectedReasons OnAddingOrUpdating(Measurement entity)
        //{
        //    if (string.IsNullOrEmpty(entity.WellName))
        //    {
        //        return RejectedReasons.WellNameEmpty;
        //    }
        //    else if (Exists(entity.Id))
        //    {
        //        return RejectedReasons.DuplicatedId;
        //    }
        //    else if (!RepositoryWrapper.Instance.Wells.ContainsName(entity.WellName))
        //    {
        //        return RejectedReasons.WellNotFound;
        //    }
        //    else if (entity.FLNADepth > entity.WaterDepth)
        //    {
        //        return RejectedReasons.FLNADepthGreaterThanWaterDepth;
        //    }
        //    return RejectedReasons.None;
        //}
    }
}
