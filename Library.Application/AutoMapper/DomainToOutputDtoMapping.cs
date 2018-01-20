using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Extensions;
using Library.Domain.Entities;

namespace Library.Application.AutoMapper
{
    public class DomainToOutputDtoMapping : Profile
    {
        public DomainToOutputDtoMapping()
        {
            CreateMap<Author, AuthorOutputDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                    src.DateOfBirth.GetCurrentAge()));

            CreateMap<Book, BookOutputDto>();
        }
    }
}
