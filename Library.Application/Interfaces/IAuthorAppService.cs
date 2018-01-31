using Library.Application.DTOs;
using Library.Infra.CrossCutting.Helpers;
using System;
using System.Collections.Generic;

namespace Library.Application.Interfaces
{
    public interface IAuthorAppService : IDisposable
    {
        PagedList<AuthorOutputDto> GetAll(int pageSize, int pageNumber);
        (bool sucess, Guid id) Add(AuthorInputDto obj);
        (bool sucess, IEnumerable<Guid> ids) AddAuthorCollection(IEnumerable<AuthorInputDto> authors);
        bool Remove(Guid id);
        bool Update(AuthorInputDto obj);
        AuthorOutputDto GetById(Guid id);
        IEnumerable<AuthorOutputDto> GetAuthorsByIds(IEnumerable<Guid> ids);
        bool AuthorExists(Guid id);
    }
}
