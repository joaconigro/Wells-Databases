using System;
using System.Collections.Generic;
using System.Linq;

namespace Wells.CorePersistence.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDbContext Context { get; set; }

        public RepositoryBase(ApplicationDbContext context)
        {
            Context = context;
        }

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public List<RejectedEntity> AddRange(IEnumerable<T> entities, IProgress<int> progress)
        {
            var rejecteds = new List<RejectedEntity>();
            var inserted = new List<T>();
            RejectedReasons reason;
            int total = entities.Count();
            for (int i = 0; i < total; i++)
            {
                var e = entities.ElementAt(i);
                reason = RejectedReasons.None;
                if (OnAddingOrUpdating(e) == RejectedReasons.None) {
                    inserted.Add(e);
                }
                else {
                    rejecteds.Add(new RejectedEntity(e, i + 2, reason));
                }
                progress.Report((i + 1) / total * 100);
            }

            Context.Set<T>().AddRange(inserted);

            return rejecteds;

        }


        public IEnumerable<T> All => Context.Set<T>().ToList();

        public bool Exists(Predicate<T> predicate)
        {
            return Context.Set<T>().ToList().Exists(predicate);
        }

        public T Find(Predicate<T> predicate)
        {
            return Context.Set<T>().ToList().Find(predicate);
        }

        public IEnumerable<T> FindAll(Predicate<T> predicate)
        {
            return Context.Set<T>().ToList().FindAll(predicate).AsQueryable();
        }

        public void Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
        }       

        public void Update(T entity)
        {
            Context.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            Context.Set<T>().UpdateRange(entities);
        }

        public abstract T Find(string id);
        public abstract T FindByName(string name);
        public abstract bool ContainsName(string name);
        public abstract bool Exists(string id);

        protected abstract RejectedReasons OnAddingOrUpdating(T entity);
    }
}
