using Library.Domain.Entities;
using Library.Domain.Interfaces;

namespace Library.Domain.Services
{
    public class AuthorService : ServiceBase<Author>, IAuthorService
    {
        public AuthorService(IAuthorRepository repository) : base (repository)
        {

        }
    }
}
