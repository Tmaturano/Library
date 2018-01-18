using AutoMapper;
using Library.Application.Interfaces;
using Library.Application.Services;
using Library.Domain.Interfaces;
using Library.Domain.Services;
using Library.Infra.Data.Context;
using Library.Infra.Data.Interfaces;
using Library.Infra.Data.Repository;
using Library.Infra.Data.UoW;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootstrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Application
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));
            services.AddScoped<IAuthorAppService, AuthorAppService>();

            //Domain
            services.AddScoped<IAuthorService, AuthorService>();


            //Infra - Data
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<LibraryContext>();
        }
    }
}
