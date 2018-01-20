using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

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

            if (!_authorService.Add(authorDto))
            {
                //Throwing an exception is expensive, but at this case, we have at the global level on Startup handling all the
                //500 Error, so to keep it in one place, I'm throwing .
                throw new Exception("Creating an author failed on save."); 
                //return StatusCode(500, "");
            }

            var authorToReturn = _mapper.Map<AuthorOutputDto>(authorDto);

            //CreatedAtRoute will contain the URI where the newly author can be found 1st parameter
            //also, the id of the generated author in 2nd parameter
            //The URI is sent through response's header Location
            return CreatedAtRoute("GetAuthor",
                new { id = authorToReturn.Id },
                authorToReturn);

        }
    }
}