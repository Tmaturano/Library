using AutoMapper;
using Library.API.Helpers;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
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

            if (!ModelState.IsValid)
            {
                // return 422
                return new UnprocessableEntityObjectResult(ModelState);
            }

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

            if (!ModelState.IsValid)
            {
                // return 422
                return new UnprocessableEntityObjectResult(ModelState);
            }

            if (!_authorAppService.AuthorExists(authorId))
                return NotFound();

            var bookForAuthor = _bookAppService.GetBookForAuthor(authorId, id);
            if (bookForAuthor == null) //upserting
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

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateBookForAuthor(Guid authorId, Guid id, 
            [FromBody]JsonPatchDocument<BookUpdateDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            if (!_authorAppService.AuthorExists(authorId))
                return NotFound();

            var book = _bookAppService.GetBookForAuthor(authorId, id);
            if (book == null)             
                return NotFound();            

            var bookToPatch = _mapper.Map<BookUpdateDto>(book);
            patchDoc.ApplyTo(bookToPatch, ModelState); // this second parameter, passing the ModelState, is necessary otherwise because if a user pass an invalid field, the application will throw an error (500)

            TryValidateModel(bookToPatch); //Trigger a validation and any error will be end on the ModelState. Patch need this to have the DTOs DataAnnotationErrors to ModelState

            if (!ModelState.IsValid)
            {
                // return 422
                return new UnprocessableEntityObjectResult(ModelState);
            }


            //Add validation

            if (!_bookAppService.Update(bookToPatch, book))
                throw new Exception($"Patching a book {id} for author {authorId} failed on save.");

            return NoContent();
        }

    }
}
