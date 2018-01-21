using Library.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Library.Domain.Interfaces
{
    public interface IBookService : IServiceBase<Book>
    {
        void AddBookForAuthor(Guid authorId, Book book);        
        Book GetBookForAuthor(Guid authorId, Guid bookId);
        IEnumerable<Book> GetBooksByAuthorId(Guid authorId);
    }
}
