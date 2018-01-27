using System;
using System.Collections.Generic;

namespace Library.Domain.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity obj);
        void Update(TEntity obj);
        void Remove(TEntity obj);
        bool Exists(Guid id);
        TEntity GetById(Guid id);
        IEnumerable<TEntity> GetAll(int pageSize, int pageNumber);
    }
}
