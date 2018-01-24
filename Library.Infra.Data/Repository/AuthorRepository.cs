using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Infra.Data.Repository
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(LibraryContext context) : base(context)
        {

        }



        public void AddBookForAuthor(Guid authorId, Book book)
        {
            var author = GetById(authorId);
            if (author == null)
                return;

            author.AddBook(book);          
        }

        public IEnumerable<Author> GetAuthorsByIds(IEnumerable<Guid> ids)
        {
            return DbSet.Where(a => ids.Contains(a.Id))
                    .OrderBy(a => a.FirstName)
                    .OrderBy(a => a.LastName)
                    .ToList();
        }
    }
}
