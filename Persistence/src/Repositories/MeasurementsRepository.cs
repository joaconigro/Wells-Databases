using System.Linq;
using Wells.Model;

namespace Wells.Persistence.Repositories
{
    public class MeasurementsRepository : RepositoryBase<Measurement>, IBussinessObjectRepository
    {
        public MeasurementsRepository(ApplicationDbContext context) : base(context) { }

        public override IQueryable<Measurement> All => Context.Measurements.ToList()
            .OrderBy(p => p.WellName)
            .ThenBy(p => p.Date)
            .AsQueryable();

        public override bool ContainsName(string name)
        {
            return true;
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
            return null;
        }

    }
}
