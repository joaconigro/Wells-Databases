﻿using System.Linq;
using Wells.YPFModel;

namespace Wells.CorePersistence.Repositories
{
    public class MeasurementsRepository : RepositoryBase<Measurement>, IBussinessObjectRepository
    {
        public MeasurementsRepository(ApplicationDbContext context) : base(context) { }

        public override bool ContainsName(string name)
        {
            return Context.Measurements.ToList().Exists((c) => c.Name == name);
        }

        public override bool Exists(string id)
        {
            return Context.Measurements.ToList().Exists((c) => c.Id == id);
        }

        public override Measurement Find(string id)
        {
            return Context.Measurements.ToList().FirstOrDefault((c) => c.Id == id);
        }

        public override Measurement FindByName(string name)
        {
            return Context.Measurements.ToList().FirstOrDefault((c) => c.Name == name);
        }

        protected override RejectedReasons OnAddingOrUpdating(Measurement entity)
        {
            if (string.IsNullOrEmpty(entity.WellName))
            {
                return RejectedReasons.WellNameEmpty;
            }
            else if (Exists(entity.Id))
            {
                return RejectedReasons.DuplicatedId;
            }
            else if (!RepositoryWrapper.Instance.Wells.ContainsName(entity.WellName))
            {
                return RejectedReasons.WellNotFound;
            }
            else if (entity.FLNADepth > entity.WaterDepth)
            {
                return RejectedReasons.FLNADepthGreaterThanWaterDepth;
            }
            return RejectedReasons.None;
        }
    }
}
