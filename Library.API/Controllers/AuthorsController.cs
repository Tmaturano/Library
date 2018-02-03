using AutoMapper;
using Library.API.Helpers;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Infra.CrossCutting.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;

        public AuthorsController(IAuthorAppService authorService, 
                                 IMapper mapper, 
                                 IUrlHelper urlHelper, 
                                 ITypeHelperService typeHelperService)
        {
            _authorService = authorService;
            _mapper = mapper;
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetAuthors")]
        public IActionResult GetAuthors(AuthorsResourceParameters authorsResourceParameters) //([FromQuery(Name = "page")]int pageNumber = 1, [FromQuery]int pageSize = 10)
        {
            //pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;
            /*
             The ModelBinding framework is smart because it will look for matching property name inside that class             
             */

            if (!_typeHelperService.TypeHasProperties<AuthorOutputDto>(authorsResourceParameters.Fields))
                return BadRequest();

            var authors = _authorService.GetAll(authorsResourceParameters);
            
            var previousPageLink = authors.HasPrevious ? CreateAuthorsResourceUri(authorsResourceParameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = authors.HasNext ? CreateAuthorsResourceUri(authorsResourceParameters,
                ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = authors.TotalCount,
                pageSize = authors.PageSize,
                currentPage = authors.CurrentPage,
                totalPages = authors.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(paginationMetadata));

            //By Default, JsonResult returns a 200 (Ok) Http Status Code
            //return new JsonResult(authorsDTO);
            return Ok(authors.ShapeData(authorsResourceParameters.Fields));
        }

        /// <summary>
        /// Creates a Uri on x-pagination header
        /// </summary>
        /// <param name="authorsResourceParameters"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string CreateAuthorsResourceUri(AuthorsResourceParameters authorsResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetAuthors",
                        new
                        {
                            fields = authorsResourceParameters.Fields,
                            searchQuery = authorsResourceParameters.SearchQuery,
                            genre = authorsResourceParameters.Genre,
                            pageNumber = authorsResourceParameters.PageNumber - 1,
                            pageSize = authorsResourceParameters.PageSize
                        });                    
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetAuthors",
                        new
                        {
                            fields = authorsResourceParameters.Fields,
                            searchQuery = authorsResourceParameters.SearchQuery,
                            genre = authorsResourceParameters.Genre,
                            pageNumber = authorsResourceParameters.PageNumber + 1,
                            pageSize = authorsResourceParameters.PageSize
                        });
                default:
                    return _urlHelper.Link("GetAuthors",
                        new
                        {
                            fields = authorsResourceParameters.Fields,
                            searchQuery = authorsResourceParameters.SearchQuery,
                            genre = authorsResourceParameters.Genre,
                            pageNumber = authorsResourceParameters.PageNumber,
                            pageSize = authorsResourceParameters.PageSize
                        });
            }
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