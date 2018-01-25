using System.ComponentModel.DataAnnotations;

namespace Library.Application.DTOs
{
    public class BookUpdateDto : BookForManipulationDto
    {
        [Required(ErrorMessage = "Description is required for updating")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}
