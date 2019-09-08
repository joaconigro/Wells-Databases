using System;
using System.Collections.Generic;
using System.Linq;

namespace Wells.CorePersistence.Repositories
{
    public interface IRepositoryBase<T>
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        IQueryable<T> All { get; }
        IQueryable<T> FindAll(Func<T, bool> predicate);
        T Find(string id);
        T Find(Func<T, bool> predicate);
        T FindByName(string name);
        bool ContainsName(string name);
        bool Exists(string id);
        bool Exists(Func<T, bool> predicate);
    }
}