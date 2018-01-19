using System;
using System.Collections.Generic;
using AutoMapper;
using Library.Application.Interfaces;
using Library.Application.ViewModels;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infra.Data.Interfaces;

namespace Library.Application.Services
{
    public class AuthorAppService : ApplicationService, IAuthorAppService
    {
        private readonly IMapper _mapper;
        private readonly IAuthorService _authorService;

        public AuthorAppService(IMapper mapper, IAuthorService authorService, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _mapper = mapper;
            _authorService = authorService;
        }

        public void Add(AuthorViewModel obj)
        {
            try
            {
                _authorService.Add(_mapper.Map<AuthorViewModel, Author>(obj));

                var saved = unitOfWork.Commit();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public bool AuthorExists(Guid id)
        {
            try
            {
                return _authorService.Exists(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Dispose()
        {
            _authorService.Dispose();
        }

        public IEnumerable<AuthorViewModel> GetAll()
        {
            try
            {
                return _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorViewModel>>(_authorService.GetAll());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AuthorViewModel GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Remove(AuthorViewModel obj)
        {
            throw new NotImplementedException();
        }

        public void Update(AuthorViewModel obj)
        {
            throw new NotImplementedException();
        }
    }
}
