using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infra.Data.Context;
using System;
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
    }
}
