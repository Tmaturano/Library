using AutoMapper;
using Library.Application.AutoMapper;
using Library.Infra.CrossCutting.IoC;
using Library.Infra.Data.Context;
using Library.Infra.Data.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;

namespace Library.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            //IConfiguration configuration
            //Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            if (env.IsDevelopment())
                builder.AddUserSecrets<Startup>();

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        
        // This method gets called by the runtime. Use this method to add services to the container.
        //https://github.com/KevinDockx/RESTfulAPIAspNetCore_Course
        public void ConfigureServices(IServiceCollection services)
        {
            var teste = Configuration["MySecret"];
            //services.AddDbContext<LibraryContext>(o => 
            //    o.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<LibraryContext>(o =>
                o.UseInMemoryDatabase("LibraryDatabase"));

            //services.AddMvc(options =>
            //{
            //    options.OutputFormatters.Remove(new XmlDataContractSerializerOutputFormatter());
            //    options.Filters.Add(new ProducesAttribute("application/json"));
            //});

            services.AddMvc(setupAction =>
            {
                //if false, the api will return responses in the default supported format.
                setupAction.ReturnHttpNotAcceptable = true; //returns a hhtp 406 if media type not supported
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });
                        
            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperConfig)));
            services.AddCors();

            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, LibraryContext libraryContext,
            ILoggerFactory logger)
        {
            logger.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //Global Error handling for API
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            libraryContext.EnsureSeedDataForContext();

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseMvc();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            // Adding dependencies from another layers (isolated from Presentation)
            NativeInjectorBootstrapper.RegisterServices(services);
        }
    }
}
