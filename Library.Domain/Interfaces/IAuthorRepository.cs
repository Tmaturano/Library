using Library.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Library.Domain.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        void AddBookForAuthor(Guid authorId, Book book);
        IEnumerable<Author> GetAuthorsByIds(IEnumerable<Guid> ids);
    }
}
