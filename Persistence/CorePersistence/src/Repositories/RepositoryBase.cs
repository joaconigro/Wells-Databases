using System;
using System.Collections.Generic;
using System.Linq;
using Wells.StandardModel.Models;

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

        public void AddRange(IEnumerable<T> entities)
        {
            //var rejecteds = new List<RejectedEntity>();
            //var inserted = new List<T>();
            //RejectedReasons reason;
            //int total = entities.Count();
            //for (int i = 0; i < total; i++)
            //{
            //    var e = entities.ElementAt(i);
            //    reason = RejectedReasons.None;
            //    if (OnAddingOrUpdating(e) == RejectedReasons.None) {
            //        inserted.Add(e);
            //    }
            //    else {
            //        rejecteds.Add(new RejectedEntity(e as IBusinessObject, i + 2, reason));
            //    }
            //    progress.Report((i + 1) / total * 100);
            //}

            Context.Set<T>().AddRange(entities);
            OnAddingOrUpdating();
            //return rejecteds;

        }

        public async void AddRangeAsync(IEnumerable<T> entities)
        {
            await Context.Set<T>().AddRangeAsync(entities);
            OnAddingOrUpdating();
        }

        public IQueryable<T> All => Context.Set<T>().AsQueryable();

        public bool Exists(Func<T, bool> predicate)
        {
            var obj = Find(predicate);
            return obj != null;
        }

        public T Find(Func<T, bool> predicate)
        {
            return All.FirstOrDefault(predicate);
        }

        public IQueryable<T> FindAll(Func<T, bool> predicate)
        {
            return All.Where(predicate).AsQueryable();
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

        protected virtual void OnAddingOrUpdating() { }
    }
}
