using System;
using System.Collections.Generic;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infra.CrossCutting.Helpers;

namespace Library.Domain.Services
{
    public class AuthorService : ServiceBase<Author>, IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository repository) : base (repository)
        {
            _authorRepository = repository;
        }

        public PagedList<Author> GetAuthorsByGenre(AuthorsResourceParameters authorsResourceParameters)
        {
            return _authorRepository.GetAuthorsByGenre(authorsResourceParameters);
        }

        public IEnumerable<Author> GetAuthorsByIds(IEnumerable<Guid> ids)
        {            
            return _authorRepository.GetAuthorsByIds(ids);
        }
    }
}
