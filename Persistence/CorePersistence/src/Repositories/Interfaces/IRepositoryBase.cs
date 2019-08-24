using System;
using System.Collections.Generic;

namespace Wells.CorePersistence.Repositories
{
    public interface IRepositoryBase<T>
    {
        void Add(T entity);
        List<RejectedEntity> AddRange(IEnumerable<T> entities, IProgress<int> progress);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        IEnumerable<T> All { get; }
        IEnumerable<T> FindAll(Predicate<T> predicate);
        T Find(string id);
        T Find(Predicate<T> predicate);
        T FindByName(string name);
        bool ContainsName(string name);
        bool Exists(string id);
        bool Exists(Predicate<T> predicate);
    }
}