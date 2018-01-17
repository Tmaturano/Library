using AutoMapper;
using Library.Application.ViewModels;
using Library.Domain.Entities;

namespace Library.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {

        public ViewModelToDomainMappingProfile()
        {
            CreateMap<AuthorViewModel, Author>()
                .ConstructUsing(a => new Author(a.FirstName, a.LastName, a.DateOfBirth, a.Genre));

            CreateMap<BookViewModel, Book>()
                .ConstructUsing(b => new Book(b.Title));
        }
    }
}
