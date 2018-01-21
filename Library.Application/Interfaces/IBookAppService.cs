﻿using Library.Application.DTOs;
using System;
using System.Collections.Generic;

namespace Library.Application.Interfaces
{
    public interface IBookAppService : IDisposable
    {
        IEnumerable<BookOutputDto> GetBooksByAuthorId(Guid authorId);
        bool AddBookForAuthor(Guid authorId, BookInputDto book);
        BookOutputDto GetBookForAuthor(Guid authorId, Guid bookId);
        bool Add(BookInputDto book);
        bool Update(BookInputDto book);
        bool Remove(BookInputDto book);
        bool Exists(Guid id);
        BookOutputDto GetById(Guid id);
        IEnumerable<BookOutputDto> GetAll();
    }
}
