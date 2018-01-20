using Library.Application.DTOs;
using System;
using System.ComponentModel.DataAnnotations;


namespace Library.Application.DTOs
{
    public class BookInputDto
    {
        [Key]
        public Guid Id { get; set; }
                
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Max length of Title is 100")]
        public string Title { get; set; }
                
        [MaxLength(500, ErrorMessage = "Max length of Description is 500")]
        public string Description { get; set; }
                
        public AuthorInputDto Author { get; set; }
    }
}
