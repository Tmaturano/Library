using Library.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Library.Domain.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        IEnumerable<Book> GetBooksByAuthorId(Guid authorId);
        Book GetBookForAuthor(Guid authorId, Guid bookId);
    }
}
