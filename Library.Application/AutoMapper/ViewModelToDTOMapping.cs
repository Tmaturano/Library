using AutoMapper;
using Library.Application.DTOs;
using Library.Application.ViewModels;
using Library.Application.Extensions;

namespace Library.Application.AutoMapper
{
    public class ViewModelToDTOMapping : Profile
    {
        public ViewModelToDTOMapping()
        {
            CreateMap<AuthorViewModel, Author>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                    src.DateOfBirth.GetCurrentAge()));
        }
    }
}
