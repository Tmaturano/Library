using System;
using System.Collections.Generic;
using Library.Domain.Entities;
using Library.Domain.Interfaces;

namespace Library.Domain.Services
{
    public class BookService : ServiceBase<Book>, IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository) : base (bookRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public void AddBookForAuthor(Guid authorId, Book book)
        {
            _authorRepository.AddBookForAuthor(authorId, book);
            //_bookRepository.AddBookForAuthor(authorId, book);
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
