﻿using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Entities;

namespace Library.Application.AutoMapper
{
    public class UpdateDtoToDomainMappingProfile : Profile
    {
        public UpdateDtoToDomainMappingProfile()
        {
            CreateMap<BookUpdateDto, Book>()
                .ConstructUsing(b => new Book(b.Title));

            CreateMap<BookUpdateDto, BookInputDto>();

            CreateMap<BookUpdateDto, BookOutputDto>();
            CreateMap<BookOutputDto, Book>()
                .ConstructUsing(b => new Book(b.Title, b.Id))
                .ForMember(b => b.Author, opt => opt.Ignore());

            CreateMap<BookOutputDto, BookUpdateDto>();
        }
    }
}
