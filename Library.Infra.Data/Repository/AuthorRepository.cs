using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infra.Data.Context;

namespace Library.Infra.Data.Repository
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(LibraryContext context) : base(context)
        {

        }
    }
}
