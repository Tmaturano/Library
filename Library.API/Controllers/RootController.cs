using Library.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Library.API.Controllers
{
    [Route("api")]
    public class RootController : Controller
    {
        private readonly IUrlHelper _urlHelper;

        public RootController(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot([FromHeader(Name = "Accept")]string mediaType)
        {
            if (mediaType == "application/vnd.tmaturano.hateoas+json")
            {
                var links = new List<LinkDto>();
                links.Add(new LinkDto(_urlHelper.Link("GetRoot", new { }),
                    rel: "self",
                    method: "GET"));

                links.Add(new LinkDto(_urlHelper.Link("GetBookForAuthor", new { }),
                    rel: "book_for_author",
                    method: "GET"));

                links.Add(new LinkDto(_urlHelper.Link("GetBooksForAuthor", new { }),
                    rel: "books_for_author",
                    method: "GET"));

                return Ok(links);
            }

            return NoContent();
        }
    }
}