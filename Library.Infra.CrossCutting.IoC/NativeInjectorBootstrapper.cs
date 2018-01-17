using Library.Domain.Interfaces;
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

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<LibraryContext>();
        }
    }
}
