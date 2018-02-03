using AutoMapper;
using Library.API.Helpers;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Library.API.Controllers
{
    [Route("api/authors/{authorId}/books")]
    public class BooksController : Controller
    {
        private readonly IBookAppService _bookAppService;
        private readonly IAuthorAppService _authorAppService;
        private readonly IMapper _mapper;
        private readonly ILogger<BooksController> _logger;
        private readonly IUrlHelper _urlHelper;

        public BooksController(IBookAppService bookAppService, 
                               IAuthorAppService authorAppService, 
                               IMapper mapper,
                               ILogger<BooksController> logger,
                               IUrlHelper urlHelper)
        {
            _bookAppService = bookAppService;
            _authorAppService = authorAppService;
            _mapper = mapper;
            _logger = logger;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetBooksForAuthor")]
        public IActionResult GetBooksForAuthor(Guid authorId)
        {
            if (!_authorAppService.AuthorExists(authorId))
                return NotFound();

            var booksForAuthor = _bookAppService.GetBooksByAuthorId(authorId);

            //HATEOAS we need to create a link for each group 
            booksForAuthor = booksForAuthor.Select(book =>
            {
                book = CreateLinksForBook(book);
                return book;
            });

            var wrapper = new LinkedCollectionResourceWrapperDto<BookOutputDto>(booksForAuthor);

            return Ok(CreateLinksForBooks(wrapper));
        }

        [HttpGet("{id}", Name = "GetBookForAuthor")]
        public IActionResult GetBookForAuthor(Guid authorId, Guid id)
        {
            if (!_authorAppService.AuthorExists(authorId))
                return NotFound();

            var bookForAuthor = _bookAppService.GetBookForAuthor(authorId, id);
            if (bookForAuthor == null)
                return NotFound();

            return Ok(CreateLinksForBook(bookForAuthor));
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
                new { authorId = bookToReturn.AuthorId, id = bookToReturn.Id },
                CreateLinksForBook(bookToReturn));
        }

        [HttpDelete("{id}", Name = "DeleteBookForAuthor")]
        public IActionResult DeleteBookForAuthor(Guid authorId, Guid id)
        {
            if (!_authorAppService.AuthorExists(authorId))
                return NotFound();

            var bookForAuthor = _bookAppService.GetBookForAuthor(authorId, id);
            if (bookForAuthor == null)
                return NotFound();


            if (!_bookAppService.Remove(id))
                throw new Exception($"Deleting a book {id} for author {authorId} failed on save.");

            _logger.LogInformation(100, $"The book {id} for author {authorId} was deleted.");

            //sucess but don't have a content
            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateBookForAuthor")]
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

        [HttpPatch("{id}", Name = "PartiallyUpdateBookForAuthor")]
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


        private BookOutputDto CreateLinksForBook(BookOutputDto book)
        {
            //HATEOAS: here we decide in which links should be returned when a consumer of the api gets back a book representation
            //Attention: in the new {id = book.Id} the anonymous type "id" must be the same of the parameter of the given method
            book.Links.Add(new LinkDto(_urlHelper.Link("GetBookForAuthor", new { id = book.Id }),
                "self", //this is the part that the consumer of the API have to know about, because that is this the will be used by the consumer to see a specific piece of functionality is offered by the api.
                "GET"));

            book.Links.Add(new LinkDto(_urlHelper.Link("DeleteBookForAuthor", new { id = book.Id }),
                "delete_book", //this is the part that the consumer of the API have to know about, because that is this the will be used by the consumer to see a specific piece of functionality is offered by the api.
                "DELETE"));

            book.Links.Add(new LinkDto(_urlHelper.Link("UpdateBookForAuthor", new { id = book.Id }),
                "udpdate_book", //this is the part that the consumer of the API have to know about, because that is this the will be used by the consumer to see a specific piece of functionality is offered by the api.
                "PUT"));

            book.Links.Add(new LinkDto(_urlHelper.Link("PartiallyUpdateBookForAuthor", new { id = book.Id }),
                "partially_update_book", //this is the part that the consumer of the API have to know about, because that is this the will be used by the consumer to see a specific piece of functionality is offered by the api.
                "PATCH"));

            return book;
        }
        
        /// <summary>
        /// this method is to add the HATEOAS support for GetBooksForAuthor to return a link to itself
        /// </summary>
        /// <param name="booksWrapper"></param>
        /// <returns></returns>
        private LinkedCollectionResourceWrapperDto<BookOutputDto> CreateLinksForBooks(
            LinkedCollectionResourceWrapperDto<BookOutputDto> booksWrapper)
        {            
            booksWrapper.Links.Add(
                new LinkDto(_urlHelper.Link("GetBooksForAuthor", new { }),
                "self",
                "GET"));

            return booksWrapper;
        }

    }
}
