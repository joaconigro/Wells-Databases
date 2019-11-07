using System.Linq;
using Wells.Model;

namespace Wells.Persistence.Repositories
{
    public class ExternalFilesRepository : RepositoryBase<ExternalFile>, IBussinessObjectRepository
    {
        public ExternalFilesRepository(ApplicationDbContext context) : base(context) { }

        public override bool ContainsName(string name)
        {
            return Exists((c) => c.Name == name);
        }

        public override bool Exists(string id)
        {
            return Exists((c) => c.Id == id);
        }

        public override ExternalFile Find(string id)
        {
            return Find((c) => c.Id == id);
        }

        public override ExternalFile FindByName(string name)
        {
            return Find((c) => c.Name == name);
        }       
    }
}
