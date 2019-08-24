using System.Linq;
using Wells.YPFModel;

namespace Wells.CorePersistence.Repositories
{
    public class ExternalFilesRepository : RepositoryBase<ExternalFile>, IBussinessObjectRepository
    {
        public ExternalFilesRepository(ApplicationDbContext context) : base(context) { }

        public override bool ContainsName(string name)
        {
            return Context.Files.ToList().Exists((c) => c.Name == name);
        }

        public override bool Exists(string id)
        {
            return Context.Files.ToList().Exists((c) => c.Id == id);
        }

        public override ExternalFile Find(string id)
        {
            return Context.Files.ToList().FirstOrDefault((c) => c.Id == id);
        }

        public override ExternalFile FindByName(string name)
        {
            return Context.Files.ToList().FirstOrDefault((c) => c.Name == name);
        }
    }
}
