using AutoMapper;
using Library.API.Helpers;
using Library.Application.AutoMapper;
using Library.Infra.CrossCutting.IoC;
using Library.Infra.Data.Context;
using Library.Infra.Data.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Linq;
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
            //var teste = Configuration["MySecret"];
            //Need to call add-migration Initial in the Data Project and uncomment the line bellow to test in a sql server environment
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
                //setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());

                var xmlDataContractSerializerInputFormatter = new XmlDataContractSerializerInputFormatter();
                xmlDataContractSerializerInputFormatter?.SupportedMediaTypes.Add("application/vnd.tmaturano.authorwithdateofdeath.full+xml");
                setupAction.InputFormatters.Add(xmlDataContractSerializerInputFormatter);
                                
                var jsonInputFormatter = setupAction.InputFormatters.OfType<JsonInputFormatter>().FirstOrDefault();
                jsonInputFormatter?.SupportedMediaTypes.Add("application/vnd.tmaturano.author.full+json");
                jsonInputFormatter?.SupportedMediaTypes.Add("application/vnd.tmaturano.authorwithdateofdeath.full+json");

                var jsonOutputFormatter = setupAction.OutputFormatters.OfType<JsonOutputFormatter>().FirstOrDefault();
                jsonOutputFormatter?.SupportedMediaTypes.Add("application/vnd.tmaturano.hateoas+json");
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
                        
            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperConfig)));
            services.AddCors();

            RegisterServices(services);

            services.AddHttpCacheHeaders(
                (expirationModelOptions =>
                {
                   expirationModelOptions.MaxAge = 60;
                }),
                (validationModelOptions =>
                {
                    validationModelOptions.AddMustRevalidate = true;
                })
            );

            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, LibraryContext libraryContext,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            /*
             LogLevel.Trace is used for most defailed log message, only valuable into a developer debugging issue
             LogLevel.Debug contains information that may be useful for debugging but it doesn't have any long term value.
             LogLevel.Information is used to track the general flow of the application. Has some long term value.
             LogLevel.Warning shoudl be used for unexpected events in the application flow (Errors for example that don't make the application stop, but we must investigate in the future
             LogLevel.Error should be used when the current flow of the application stop because of an unhandled error.
             LogLevel.Critical should be used for any system crashes 
             */
            loggerFactory.AddDebug(LogLevel.Information);                        

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
                        var exceptionhandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionhandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");

                            //The first parameter is the code(or Id) of the error
                            logger.LogError(500, exceptionhandlerFeature.Error, exceptionhandlerFeature.Error.Message);
                        }

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            libraryContext.EnsureSeedDataForContext();
               
            app.UseResponseCaching();

            app.UseHttpCacheHeaders();

            app.UseMvc();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            // Adding dependencies from another layers (isolated from Presentation)
            NativeInjectorBootstrapper.RegisterServices(services);

            //the context where the action runs. UrlHelper will use this
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //generate a url to an action
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddTransient<ITypeHelperService, TypeHelperService>();
        }
    }
}
