﻿using Library.Application.DTOs;
using Library.Infra.CrossCutting.Helpers;
using System;
using System.Collections.Generic;

namespace Library.Application.Interfaces
{
    public interface IBookAppService : IDisposable
    {
        IEnumerable<BookOutputDto> GetBooksByAuthorId(Guid authorId);
        (bool sucess, Guid id) AddBookForAuthor(Guid authorId, BookInputDto book);        
        BookOutputDto GetBookForAuthor(Guid authorId, Guid bookId);
        bool Add(BookInputDto book);
        bool Update(BookUpdateDto bookUpdate, BookOutputDto bookOutput);
        bool Remove(Guid id);
        bool Exists(Guid id);
        BookOutputDto GetById(Guid id);
        PagedList<BookOutputDto> GetAll(int pageSize, int pageNumber);
    }
}
