using AutoMapper;
using Library.API.Helpers;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.API.Controllers
{
    [Route("api/authors")]
    public class AuthorsController : Controller
    {
        private readonly IAuthorAppService _authorService;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorAppService authorService, IMapper mapper)
        {
            _authorService = authorService;
            _mapper = mapper;
        }

        [HttpGet()]
        public IActionResult GetAuthors()
        {
            var authors = _authorService.GetAll();

            //By Default, JsonResult returns a 200 (Ok) Http Status Code
            //return new JsonResult(authorsDTO);
            return Ok(authors);
        }

        [HttpGet("{id}", Name = "GetAuthor")]
        public IActionResult GetAuthor(Guid id)
        {
            var author = _authorService.GetById(id);

            if (author == null)
                return NotFound();
            

            return Ok(author);
        }

        [HttpPost()]
        public IActionResult CreateAuthor([FromBody]AuthorInputDto authorDto)
        {
            if (authorDto == null)
                return BadRequest();

            var result = _authorService.Add(authorDto);
            if (!result.sucess)
            {
                //Throwing an exception is expensive, but at this case, we have at the global level on Startup handling all the
                //500 Error, so to keep it in one place, I'm throwing .
                throw new Exception("Creating an author failed on save."); 
                //return StatusCode(500, "");
            }

            var authorToReturn = _mapper.Map<AuthorOutputDto>(authorDto);
            authorToReturn.Id = result.id;

            //CreatedAtRoute will contain the URI where the newly author can be found 1st parameter
            //also, the id of the generated author in 2nd parameter
            //The URI is sent through response's header Location
            return CreatedAtRoute("GetAuthor",
                new { id = authorToReturn.Id },
                authorToReturn);
        }

        [HttpPost("createauthors")]
        public IActionResult CreateAuthorCollection([FromBody]IEnumerable<AuthorInputDto> authorsCollection)
        {
            if (authorsCollection == null)
                return BadRequest();

            var result = _authorService.AddAuthorCollection(authorsCollection);

            if (!result.sucess)
            {
                throw new Exception("Creating authors collection failed on save");
            }

            var authorsToReturn = _mapper.Map<IEnumerable<AuthorInputDto>, IEnumerable<AuthorOutputDto>>(authorsCollection);
            var ids = string.Join(",", result.ids);

            return CreatedAtRoute("GetAuthorCollection",
                new { ids = ids },
                authorsToReturn);            
        }

        [HttpGet("({ids})", Name = "GetAuthorCollection")]
        public IActionResult GetAuthorCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
                return BadRequest();

            var authors = _authorService.GetAuthorsByIds(ids);
            if (ids.Count() != authors.Count())
                return NotFound();

            return Ok(authors);
        }

        [HttpPatch("{id}")]
        public IActionResult BlockAuthorCreation(Guid id)
        {
            if (_authorService.AuthorExists(id))
                return new StatusCodeResult(StatusCodes.Status409Conflict);

            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(Guid id)
        {            
            if (!_authorService.AuthorExists(id))
                return NotFound();

            if (!_authorService.Remove(id))
                throw new Exception($"Deleting the author {id} failed on save");

            return NoContent();
        }
    }
}