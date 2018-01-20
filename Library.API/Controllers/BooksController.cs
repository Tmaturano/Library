using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Library.API.Controllers
{
    [Route("api/authors/{authorId}/books")]
    public class BooksController : Controller
    {
        private readonly IBookAppService _bookAppService;
        private readonly IAuthorAppService _authorAppService;
        
        public BooksController(IBookAppService bookAppService, IAuthorAppService authorAppService)
        {
            _bookAppService = bookAppService;
            _authorAppService = authorAppService;            
        }

        [HttpGet()]
        public IActionResult GetBooksForAuthor(Guid authorId)
        {
            if (!_authorAppService.AuthorExists(authorId))
                return NotFound();

            var books = _bookAppService.GetBooksByAuthorId(authorId);            
            return Ok(books);
        }

        [HttpGet("{bookId}")]
        public IActionResult GetBookForAuthor(Guid authorId, Guid bookId)
        {
            if (!_authorAppService.AuthorExists(authorId))
                return NotFound();

            var bookForAuthor = _bookAppService.GetBookForAuthor(authorId, bookId);
            if (bookForAuthor == null)
                return NotFound();
                       
            return Ok(bookForAuthor);
        }
    }
}
