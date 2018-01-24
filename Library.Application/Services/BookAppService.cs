using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Entities;
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
        public bool Add(BookInputDto book)
        {
            try
            {
                _bookService.Add(_mapper.Map<BookInputDto, Book>(book));
                return Commit();
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public bool Remove(Guid id)
        {
            try
            {
                var book = _bookService.GetById(id);
                _bookService.Remove(book);

                return Commit();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Update(BookUpdateDto bookUpdate, BookOutputDto bookOutput)
        {
            try
            {
                var bookUpdated = _mapper.Map(bookUpdate, bookOutput);
                var book = _mapper.Map<BookOutputDto, Book>(bookUpdated);

                _bookService.Update(book);
                return Commit();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        public (bool sucess, Guid id) AddBookForAuthor(Guid authorId, BookInputDto book)
        {
            try
            {
                var bookEntity = _mapper.Map<BookInputDto, Book>(book);
                _bookService.AddBookForAuthor(authorId, bookEntity);

                return (Commit(), bookEntity.Id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Search
        public bool Exists(Guid id)
        {
            try
            {
                return _bookService.Exists(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<BookOutputDto> GetAll()
        {
            try
            {
                return _mapper.Map<IEnumerable<Book>, IEnumerable<BookOutputDto>>(_bookService.GetAll());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public BookOutputDto GetBookForAuthor(Guid authorId, Guid bookId)
        {
            try
            {
                var book = _bookService.GetBookForAuthor(authorId, bookId);
                return _mapper.Map<Book, BookOutputDto>(book);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<BookOutputDto> GetBooksByAuthorId(Guid authorId)
        {
            try
            {
                var books = _bookService.GetBooksByAuthorId(authorId);
                return _mapper.Map<IEnumerable<Book>, IEnumerable<BookOutputDto>>(books);
            }
            catch (Exception)
            {

                throw;
            }
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
