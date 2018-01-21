using Library.Application.DTOs;
using System;
using System.Collections.Generic;

namespace Library.Application.Interfaces
{
    public interface IAuthorAppService : IDisposable
    {
        IEnumerable<AuthorOutputDto> GetAll();
        (bool sucess, Guid id) Add(AuthorInputDto obj);
        bool Remove(AuthorInputDto obj);
        bool Update(AuthorInputDto obj);
        AuthorOutputDto GetById(Guid id);
        bool AuthorExists(Guid id);
    }
}
