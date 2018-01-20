using Library.Application.DTOs;
using System;
using System.Collections.Generic;

namespace Library.Application.Interfaces
{
    public interface IBookAppService : IDisposable
    {
        IEnumerable<BookOutputDto> GetBooksByAuthorId(Guid authorId);
        BookOutputDto GetBookForAuthor(Guid authorId, Guid bookId);
        void Add(BookInputDto book);
        void Update(BookInputDto book);
        void Remove(BookInputDto book);
        bool Exists(Guid id);
        BookOutputDto GetById(Guid id);
        IEnumerable<BookOutputDto> GetAll();
    }
}
