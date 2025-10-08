using Application.Interface.ContentInterface;
using Application.Interface.Mappers;
using Application.Interface.UserInterface;
using Application.Interface.UtilsInterface;
using Application.Services;
using Application.Utils;
using Domain.Interface.HttpContext;
using Domain.Interface.Mappers;
using Domain.Interface.Repository;
using Infraestructure.Data;
using Infraestructure.HttpAcessor;
using Infraestructure.Repository;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReelfyAPI.Data;
using ReelfyAPI.Services;
using ReelfyAPI.Utils;
using System.Text;

namespace ReelfyAPI.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthServices, AuthService>();
        services.AddScoped<IContentService, ContentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IContentListService, ContentListService>();
        services.AddScoped<IPreferenceService, PreferenceService>();
        services.AddScoped<IUserMapper, UserMapper>();
        services.AddScoped<ICastMapper, CastMapper>();
        services.AddScoped<ICrewMapper, CrewMapper>();
        services.AddScoped<IGenreMapper, GenreMapper>();
        services.AddScoped<IStreamingMapper, StreamingMapper>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IContentRepository, ContentRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IPreferenceRepository, PreferenceRepository>();
        services.AddScoped<IStreamingRepository, StreamingRepository>();
        services.AddScoped<ICrewRepository, CrewRepository>();
        services.AddScoped<ICastRepository, CastRepository>();
        services.AddScoped<IContentsListRepository, ContentsListRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IContextUser, ContextUser>();
        services.AddScoped<JwtService>();

        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Infrastructure"));
        });

        return services;
    }

    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddMemoryCache();

        return services;
    }

    public static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Reelfy API", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Insira o token JWT aqui",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        configuration.GetSection("AppSettings:Token").Value!)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        return services;
    }
}