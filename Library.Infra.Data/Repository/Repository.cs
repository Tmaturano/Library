using Library.Domain.Interfaces;
using Library.Infra.CrossCutting.Helpers;
using Library.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Infra.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly LibraryContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(LibraryContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }

        public void Add(TEntity obj)
        {            
            DbSet.Add(obj);
        }

        public virtual bool Exists(Guid id)
        {            
            return GetById(id) != null;
        }

        public virtual PagedList<TEntity> GetAll(int pageSize, int pageNumber)
        {
            var collectionBeforePaging = DbSet.ToList();

            return PagedList<TEntity>.Create(collectionBeforePaging, pageNumber, pageSize);
        }

        public TEntity GetById(Guid id)
        {            
            return DbSet.Find(id);
        }

        public void Remove(TEntity obj)
        {
            DbSet.Remove(obj);
        }

        public void Update(TEntity obj)
        {
            DbSet.Update(obj);
        }

        public void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
