using Library.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Library.Domain.Interfaces
{
    public interface IAuthorService : IServiceBase<Author>
    {
        IEnumerable<Author> GetAuthorsByIds(IEnumerable<Guid> ids);
    }
}
