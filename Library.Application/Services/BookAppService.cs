using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Interfaces;
using Library.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Library.Application.Services
{
    public class BookAppService : ApplicationService, IBookAppService
    {
        private readonly IMapper _mapper;
        private readonly IBookService _bookService;

        public BookAppService(IMapper mapper, IBookService bookService, IUnitOfWork unitOfWork) : base (unitOfWork)
        {
            _mapper = mapper;
            _bookService = bookService;
        }

        #region Persistance
        public void Add(BookInputDto book)
        {
            throw new NotImplementedException();
        }
        
        public void Remove(BookInputDto book)
        {
            throw new NotImplementedException();
        }

        public void Update(BookInputDto book)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Search
        public bool Exists(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BookOutputDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public BookOutputDto GetBookForAuthor(Guid authorId, Guid bookId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BookOutputDto> GetBooksByAuthorId(Guid authorId)
        {
            throw new NotImplementedException();
        }

        public BookOutputDto GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        #endregion

        public void Dispose()
        {
            _bookService.Dispose();
        }
    }
}
