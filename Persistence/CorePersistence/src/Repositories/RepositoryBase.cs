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

        public event EntityRemovedEventHandler<T> OnEntityRemoved;

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {        
            Context.Set<T>().AddRange(entities);
            OnAddingOrUpdating();
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
            OnEntityRemoved?.Invoke(this, new EntityRemovedEventArgs<T>(entity));
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
            OnEntityRemoved?.Invoke(this, new EntityRemovedEventArgs<T>(entities));
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

    public class EntityRemovedEventArgs<T> : EventArgs where T : class
    {
        T Entity { get; }

        IEnumerable<T> Entities { get; }

        public EntityRemovedEventArgs(T entity) { Entity = entity; }

        public EntityRemovedEventArgs(IEnumerable<T> entities) { Entities = entities; }
    }

    public delegate void EntityRemovedEventHandler<T>(object sender, EntityRemovedEventArgs<T> entityArgs) where T : class;
}
