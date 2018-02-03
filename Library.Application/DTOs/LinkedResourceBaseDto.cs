using System.Collections.Generic;

namespace Library.Application.DTOs
{
    /// <summary>
    /// HATEOAS base class
    /// </summary>
    public abstract class LinkedResourceBaseDto
    {
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}
