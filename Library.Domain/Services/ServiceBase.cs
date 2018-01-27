using Library.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Library.Domain.Services
{
    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class
    {
        private readonly IRepository<TEntity> _repository;

        public ServiceBase(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual void Add(TEntity obj)
        {
            _repository.Add(obj);
        }

        public void Remove(TEntity obj)
        {
            _repository.Remove(obj);
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        public IEnumerable<TEntity> GetAll(int pageSize, int pageNumber)
        {
            return _repository.GetAll(pageSize, pageNumber);
        }

        public TEntity GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        public void Update(TEntity obj)
        {
            _repository.Update(obj);
        }

        public bool Exists(Guid id)
        {
            return _repository.Exists(id);
        }
    }
}
