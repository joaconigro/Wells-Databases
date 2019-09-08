﻿using Wells.YPFModel;
using System.Linq;

namespace Wells.CorePersistence.Repositories
{
    public class AnalysesRepository : RepositoryBase<ChemicalAnalysis>, IBussinessObjectRepository
    {
        public AnalysesRepository(ApplicationDbContext context) : base(context) { }

        public override bool ContainsName(string name)
        {
            return Exists((c) => c.Name == name);
        }

        public override bool Exists(string id)
        {
            return Exists((c) => c.Id == id);
        }

        public override ChemicalAnalysis Find(string id)
        {
            return Find((c) => c.Id == id);
        }

        public override ChemicalAnalysis FindByName(string name)
        {
            return Find((c) => c.Name == name);
        }

        //protected override RejectedReasons OnAddingOrUpdating(ChemicalAnalysis entity)
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
        //    return RejectedReasons.None;
        //}
    }
}
