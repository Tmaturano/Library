using System;
using System.Collections.Generic;
using Library.Domain.Entities;
using Library.Domain.Interfaces;

namespace Library.Domain.Services
{
    public class AuthorService : ServiceBase<Author>, IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository repository) : base (repository)
        {
            _authorRepository = repository;
        }

        public IEnumerable<Author> GetAuthorsByIds(IEnumerable<Guid> ids)
        {            
            return _authorRepository.GetAuthorsByIds(ids);
        }
    }
}
