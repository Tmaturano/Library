using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infra.CrossCutting.Helpers;
using Library.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;

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

        #region Persistance
        
        public (bool sucess, Guid id) Add(AuthorInputDto obj)
        {
            try
            {
                var author = _mapper.Map<AuthorInputDto, Author>(obj);
                _authorService.Add(author);
                

                return (unitOfWork.Commit(), author.Id);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public (bool sucess, IEnumerable<Guid> ids) AddAuthorCollection(IEnumerable<AuthorInputDto> authors)
        {
            try
            {
                var ids = new List<Guid>();
                foreach (var author in authors)
                {
                    var authorEntity = _mapper.Map<AuthorInputDto, Author>(author);
                    _authorService.Add(authorEntity);
                    ids.Add(authorEntity.Id);
                }

                return (unitOfWork.Commit(), ids);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public bool Remove(Guid id)
        {
            try
            {
                var author = _authorService.GetById(id);
                _authorService.Remove(author);

                return Commit();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Update(AuthorInputDto obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Search

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

        public PagedList<AuthorOutputDto> GetAll(int pageSize, int pageNumber)
        {
            try
            {
                var authors = _authorService.GetAll(pageSize, pageNumber);

                /*
                 Needed to do that because was not possible to use automapper to map from PagedList to PagedList
                 */
                var authorsOutputDto = new List<AuthorOutputDto>();
                foreach (var author in authors)
                {                    
                    authorsOutputDto.Add(_mapper.Map<Author, AuthorOutputDto>(author));
                }

                return new PagedList<AuthorOutputDto>(authorsOutputDto, authors.TotalCount, pageNumber, pageSize);                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AuthorOutputDto GetById(Guid id)
        {
            try
            {
                return _mapper.Map<Author, AuthorOutputDto>(_authorService.GetById(id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<AuthorOutputDto> GetAuthorsByIds(IEnumerable<Guid> ids)
        {
            try
            {
                var authors = _authorService.GetAuthorsByIds(ids);
                return _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorOutputDto>>(authors);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        public void Dispose()
        {
            _authorService.Dispose();
        }
    }
}
