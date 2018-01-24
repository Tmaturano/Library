using Library.Application.DTOs;
using System;
using System.Collections.Generic;

namespace Library.Application.Interfaces
{
    public interface IAuthorAppService : IDisposable
    {
        IEnumerable<AuthorOutputDto> GetAll();
        (bool sucess, Guid id) Add(AuthorInputDto obj);
        (bool sucess, IEnumerable<Guid> ids) AddAuthorCollection(IEnumerable<AuthorInputDto> authors);
        bool Remove(AuthorInputDto obj);
        bool Update(AuthorInputDto obj);
        AuthorOutputDto GetById(Guid id);
        IEnumerable<AuthorOutputDto> GetAuthorsByIds(IEnumerable<Guid> ids);
        bool AuthorExists(Guid id);
    }
}
