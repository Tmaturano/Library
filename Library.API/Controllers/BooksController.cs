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
        private readonly IMapper _mapper;

        public BooksController(IBookAppService bookAppService, IAuthorAppService authorAppService, IMapper mapper)
        {
            _bookAppService = bookAppService;
            _authorAppService = authorAppService;
            _mapper = mapper;
        }

        [HttpGet()]
        public IActionResult GetBooksForAuthor(Guid authorId)
        {
            if (!_authorAppService.AuthorExists(authorId))
                return NotFound();

            var books = _bookAppService.GetBooksByAuthorId(authorId);
            return Ok(books);
        }

        [HttpGet("{bookId}", Name = "GetBookForAuthor")]
        public IActionResult GetBookForAuthor(Guid authorId, Guid bookId)
        {
            if (!_authorAppService.AuthorExists(authorId))
                return NotFound();

            var bookForAuthor = _bookAppService.GetBookForAuthor(authorId, bookId);
            if (bookForAuthor == null)
                return NotFound();

            return Ok(bookForAuthor);
        }

        [HttpPost()]
        public IActionResult CreateBookForAuthor(Guid authorId, [FromBody]BookInputDto bookInputDto)
        {
            if (bookInputDto == null)
                return BadRequest();

            if (!_authorAppService.AuthorExists(authorId))
                return NotFound();

            var result = _bookAppService.AddBookForAuthor(authorId, bookInputDto);

            if (!result.sucess)
                throw new Exception($"Creating a book for author {authorId} failed on save.");

            var bookToReturn = _mapper.Map<BookOutputDto>(bookInputDto);
            bookToReturn.Id = result.id;
            bookToReturn.AuthorId = authorId;
            return CreatedAtRoute("GetBookForAuthor",
                new { authorId = bookToReturn.AuthorId, bookId = bookToReturn.Id },
                bookToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBookForAuthor(Guid authorId, Guid id)
        {
            if (!_authorAppService.AuthorExists(authorId))
                return NotFound();

            var bookForAuthor = _bookAppService.GetBookForAuthor(authorId, id);
            if (bookForAuthor == null)
                return NotFound();


            if (!_bookAppService.Remove(id))
                throw new Exception($"Deleting a book {id} for author {authorId} failed on save.");

            //sucess but don't have a content
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBookForAuthor(Guid authorId, Guid id, [FromBody]BookUpdateDto book)
        {
            if (book == null)
                return BadRequest();

            if (!_authorAppService.AuthorExists(authorId))
                return NotFound();

            var bookForAuthor = _bookAppService.GetBookForAuthor(authorId, id);
            if (bookForAuthor == null)
            {
                var bookInputDto = _mapper.Map<BookInputDto>(book);
                var result = _bookAppService.AddBookForAuthor(authorId, bookInputDto);

                if (!result.sucess)
                    throw new Exception($"Upserting a book for author {authorId} failed on save.");

                var bookToReturn = _mapper.Map<BookOutputDto>(bookInputDto);
                bookToReturn.Id = result.id;
                bookToReturn.AuthorId = authorId;

                return CreatedAtRoute("GetBookForAuthor",
                    new { authorId = bookToReturn.AuthorId, bookId = bookToReturn.Id },
                    bookToReturn);
            }

            if (!_bookAppService.Update(book, bookForAuthor))
                throw new Exception($"Updating a book {id} for author {authorId} failed on save.");

            return NoContent();
        }

    }
}
