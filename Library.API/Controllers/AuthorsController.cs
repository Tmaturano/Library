using Library.Application.Extensions;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AutoMapper;
using Library.Application.ViewModels;
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
            var authorsDTO = _mapper.Map<IEnumerable<AuthorViewModel>, IEnumerable<Author>>(authors);


            //By Default, JsonResult returns a 200 (Ok) Http Status Code
            //return new JsonResult(authorsDTO);
            return Ok(authorsDTO);
        }

        [HttpGet("{id}")]
        public IActionResult GetAuthor(Guid id)
        {
            var author = _authorService.GetById(id);

            if (author == null)
                return NotFound();

            var authorDTO = _mapper.Map<AuthorViewModel, Author>(author);

            return Ok(authorDTO);
        }
    }
}