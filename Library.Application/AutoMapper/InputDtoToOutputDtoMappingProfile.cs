using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Extensions;

namespace Library.Application.AutoMapper
{
    public class InputDtoToOutputDtoMappingProfile : Profile
    {
        public InputDtoToOutputDtoMappingProfile()
        {
            CreateMap<AuthorInputDto, AuthorOutputDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                    src.DateOfBirth.GetCurrentAge()));

            CreateMap<BookInputDto, BookOutputDto>();
        }
    }
}
