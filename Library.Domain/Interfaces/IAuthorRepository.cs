using Library.Domain.Entities;
using System;

namespace Library.Domain.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        void AddBookForAuthor(Guid authorId, Book book);
    }
}
