using System;
using System.Collections.Generic;
using Library.Domain.Entities;
using Library.Domain.Interfaces;

namespace Library.Domain.Services
{
    public class BookService : ServiceBase<Book>, IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository) : base (bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Book GetBookForAuthor(Guid authorId, Guid bookId)
        {
            return _bookRepository.GetBookForAuthor(authorId, bookId);
        }

        public IEnumerable<Book> GetBooksByAuthorId(Guid authorId)
        {
            return _bookRepository.GetBooksByAuthorId(authorId);
        }
    }
}
