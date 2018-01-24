using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Application.DTOs
{
    public class AuthorInputDto
    {                
        [Required(ErrorMessage = "FirstName is required")]
        [MaxLength(50, ErrorMessage = "Max length is 50")]
        public string FirstName { get; set; }
                
        [Required(ErrorMessage = "LastName is required")]
        [MaxLength(50, ErrorMessage = "Max length is 50")]
        public string LastName { get; set; }
                
        [Required(ErrorMessage = "DateOfBirth is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date, ErrorMessage = "DateOfBirth format is invalid")]
        public DateTimeOffset DateOfBirth { get; set; }
                
        [Required(ErrorMessage = "Genre is required")]
        public string Genre { get; set; }

        public ICollection<BookInputDto> Books { get; set; } = new List<BookInputDto>();
                
    }
}
