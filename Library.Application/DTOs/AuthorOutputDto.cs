using System;

namespace Library.Application.DTOs
{
    public class AuthorOutputDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Genre { get; set; }

    }
}
