using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Entities;

namespace Library.Application.AutoMapper
{
    public class DomainToOutputDtoMappingProfile : Profile
    {
        public DomainToOutputDtoMappingProfile()
        {
            CreateMap<Author, AuthorOutputDto>();
            CreateMap<Book, BookOutputDto>();
        }
    }
}
