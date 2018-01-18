using System;
using System.Collections.Generic;

namespace Library.Domain.Interfaces
{
    public interface IServiceBase<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity obj);
        void Remove(TEntity obj);
        void Update(TEntity obj);
        IEnumerable<TEntity> GetAll();
        TEntity GetById(Guid id);
    }
}
