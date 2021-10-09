using Catalog.API.Filters;
using Exchange.API.Contracts.Requests;
using Exchange.API.Contracts.Validations;
using Exchange.Data;
using Exchange.Data.Interfaces;
using Exchange.Data.Repositories;
using Exchange.Data.UoW;
using Exchange.Service;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Service.Common.Converters;
using System.Linq;

namespace Exchange.API.Configurations
{
    public static class ContainerSetup
    {
        public static void Setup(this IServiceCollection services, IConfiguration configuration)
        {
            AddMvc(services);
            AddApiDocs(services);
            ConfigureDatabaseProvider(services, configuration);
            AddAbstract(services);
            AddServices(services);
        }


        public static void AddMvc(IServiceCollection services)
        {
            // API Controllers
            services.AddControllers();
            services.AddMvc(options =>
            {
                options.Filters.Add(new ApiExceptionFilterAttribute());
            }).AddJsonOptions(options => {
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            // override modelstate
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).ToList();
                    var result = new
                    {
                        success = false,
                        Errors = errors
                    };
                    return new BadRequestObjectResult(result);
                };
            });
        }

        public static void AddApiDocs(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Exchange API", Version = "v1" });
            });
        }

        public static void ConfigureDatabaseProvider(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<ExchangeDbContext>(
            //    options => options.UseSqlServer(
            //        configuration.GetConnectionString("DefaultConnection"),
            //        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Exchange")
            //    )
            //);

            services.AddDbContext<ExchangeDbContext>(options =>
            {
                options.UseInMemoryDatabase("ExchangeInMemoryDB");
            });
        }

        public static void AddAbstract(IServiceCollection services)
        {
            //Register Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork<ExchangeDbContext>>();

        }

        public static void AddServices(IServiceCollection services)
        {
            // Services
            services.AddTransient<ICurrencyService, CurrencyService>();
            services.AddTransient<IExchangeRateService, ExchangeRateService>();
            services.AddTransient<IQuoteExchangeRateService, QuoteExchangeRateService>();

        }

        public static void AddValidator(IServiceCollection services)
        {
            // Validators
            services.AddSingleton<IValidator<CurrencyCreateRequest>, CurrencyCreateValidator>();
            services.AddSingleton<IValidator<CurrencyUpdateRequest>, CurrencyUpdateValidator>();

            services.AddSingleton<IValidator<ExchangeRateCreateRequest>, ExchangeRateCreateValidator>();
            services.AddSingleton<IValidator<ExchangeRateUpdateRequest>, ExchangeRateUpdateValidator>();

            services.AddSingleton<IValidator<QuoteExchangeRateCreateRequest>, QuoteExchangeRateCreateValidator>();
        }
    }
}
