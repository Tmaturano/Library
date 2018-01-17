using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Infra.Data.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(LibraryContext context) : base(context)
        {

        }
        public Book GetBookForAuthor(Guid authorId, Guid bookId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Book> GetBooksByAuthorId(Guid authorId)
        {
            return DbSet.AsNoTracking().Where(a => a.AuthorId == authorId).OrderBy(a => a.Title);
        }
    }
}
