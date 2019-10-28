using System.Linq;
using Wells.Model;

namespace Wells.Persistence.Repositories
{
    public class MeasurementsRepository : RepositoryBase<Measurement>, IBussinessObjectRepository
    {
        public MeasurementsRepository(ApplicationDbContext context) : base(context) { }

        public override bool ContainsName(string name)
        {
            return true; // Exists((c) => c.Name == name);
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
            return null; // Find((c) => c.Name == name);
        }

    }
}
