using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Entities;

namespace Library.Application.AutoMapper
{
    public class InputDtoToDomainMappingProfile : Profile
    {

        public InputDtoToDomainMappingProfile()
        {
            CreateMap<AuthorInputDto, Author>()
                .ConstructUsing(a => new Author(a.FirstName, a.LastName, a.DateOfBirth, a.Genre));

            CreateMap<AuthorInputWithDateOfDeathDto, Author>()
                .ConstructUsing(a => new Author(a.FirstName, a.LastName, a.DateOfBirth, a.Genre));

            CreateMap<BookInputDto, Book>()
                .ConstructUsing(b => new Book(b.Title));
        }
    }
}
