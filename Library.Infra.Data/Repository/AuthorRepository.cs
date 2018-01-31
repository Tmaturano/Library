using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infra.CrossCutting.Helpers;
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

        public PagedList<Author> GetAuthorsByFilter(AuthorsResourceParameters authorsResourceParameters)
        {
            var collectionBeforePaging = DbSet.OrderBy(a => a.FirstName)
                                              .ThenBy(a => a.LastName).AsQueryable();

            if (!string.IsNullOrWhiteSpace(authorsResourceParameters.Genre))
            {
                var genreForWhereClause = authorsResourceParameters.Genre.Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging.Where(a => a.Genre.ToLowerInvariant() == genreForWhereClause);
            }

            if (!string.IsNullOrWhiteSpace(authorsResourceParameters.SearchQuery))
            {
                var searchForWhereClause = authorsResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging.Where(
                                            a => a.Genre.ToLowerInvariant().Contains(searchForWhereClause)
                                            || a.FirstName.ToLowerInvariant().Contains(searchForWhereClause)
                                            || a.LastName.ToLowerInvariant().Contains(searchForWhereClause));
            }

            return PagedList<Author>.Create(collectionBeforePaging, authorsResourceParameters.PageNumber, authorsResourceParameters.PageSize);
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
