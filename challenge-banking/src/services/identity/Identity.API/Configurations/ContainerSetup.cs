using Identity.API.Filters;
using Identity.API.Contracts.Requests;
using Identity.API.Contracts.Validations;
using Identity.Data;
using Identity.Data.Interfaces;
using Identity.Data.Repositories;
using Identity.Data.UoW;
using Identity.Service;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Service.Common.Converters;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using Identity.API.Helpers;

namespace Identity.API.Configurations
{
    public static class ContainerSetup
    {
        public static void Setup(this IServiceCollection services, IConfiguration configuration)
        {
            AddMvc(services);
            AddApiDocs(services);
            ConfigureDatabaseProvider(services, configuration);
            AddAuthentication(services, configuration);
            AddCofigureCors(services);
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
            }).AddJsonOptions(options =>
            {
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

        public static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            // configure strongly typed settings objects
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var secretKey = Encoding.ASCII.GetBytes(appSettings.SecretKey);

            services.AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(b =>
            {
                b.RequireHttpsMetadata = false;
                b.SaveToken = true;
                b.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public static void AddCofigureCors(IServiceCollection services)
        {
            services.AddCors();
        }

        public static void AddApiDocs(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity API", Version = "v1" });
                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization using JWT bearer security scheme"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id ="bearerAuth",
                            }
                        },
                        new string[]{}
                    }
                });
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

            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseInMemoryDatabase("IdentityInMemoryDB");
            });
        }

        public static void AddAbstract(IServiceCollection services)
        {
            //Register Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork<IdentityDbContext>>();

        }

        public static void AddServices(IServiceCollection services)
        {
            // Services
            services.AddTransient<IUserService, UserService>();
        }

        public static void AddValidator(IServiceCollection services)
        {
            // Validators
            services.AddSingleton<IValidator<UserCreateRequest>, UserCreateValidator>();
            services.AddSingleton<IValidator<UserUpdateRequest>, UserUpdateValidator>();
        }
    }
}
