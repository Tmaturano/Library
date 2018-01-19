using Library.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Library.Application.Interfaces
{
    public interface IAuthorAppService : IDisposable
    {
        IEnumerable<AuthorViewModel> GetAll();
        void Add(AuthorViewModel obj);
        void Remove(AuthorViewModel obj);
        void Update(AuthorViewModel obj);        
        AuthorViewModel GetById(Guid id);
        bool AuthorExists(Guid id);
    }
}
