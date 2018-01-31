using Library.Infra.CrossCutting.Helpers;
using System;
using System.Collections.Generic;

namespace Library.Domain.Interfaces
{
    public interface IServiceBase<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity obj);
        void Remove(TEntity obj);
        void Update(TEntity obj);
        PagedList<TEntity> GetAll(int pageSize, int pageNumbers);
        TEntity GetById(Guid id);
        bool Exists(Guid id);
    }
}
