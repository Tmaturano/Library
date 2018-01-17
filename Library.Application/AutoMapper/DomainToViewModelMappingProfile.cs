using AutoMapper;
using Library.Application.ViewModels;
using Library.Domain.Entities;

namespace Library.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Author, AuthorViewModel>();
            CreateMap<Book, BookViewModel>();
        }
    }
}
