using Application.Interface.ContentInterface;
using Application.Interface.Mappers;
using Application.Interface.UserInterface;
using Application.Services;
using Application.Utils;
using BGS;
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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;
using ReelfyAPI.Data;
using ReelfyAPI.Services;
using ReelfyAPI.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddScoped<IAuthServices, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IContentRepository, ContentRepository>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IMemoryCache, MemoryCache>();
builder.Services.AddScoped<JwtService, JwtService>();
builder.Services.AddScoped<IUserMapper, UserMapper>();
builder.Services.AddScoped<IContentMapper, ContentMapper>();
builder.Services.AddScoped<ICastMapper, CastMapper>();
builder.Services.AddScoped<ICrewMapper, CrewMapper>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IStreamingMapper, StreamingMapper>();
builder.Services.AddScoped<IPreferenceRepository, PreferenceRepository>();
builder.Services.AddScoped<IPreferenceService, PreferenceService>();
builder.Services.AddScoped<IContextUser, ContextUser>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IGenreMapper, GenreMapper>();
builder.Services.AddScoped<IStreamingRepository, StreamingRepository>();
builder.Services.AddScoped<ICrewRepository, CrewRepository>();
builder.Services.AddScoped<ICastRepository, CastRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IContentListService, ContentListService>();
builder.Services.AddScoped<IContentsListRepository, ContentsListRepository>();

builder.Services.AddHostedService<CleaningDatabase>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Minha API", Version = "v1" });

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
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Infrastructure"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/problem+json";

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Não autorizado",
                    Detail = "Você precisa de um token de autenticação válido para acessar este recurso.",
                    Instance = context.Request.Path
                };

                return context.Response.WriteAsJsonAsync(problemDetails);
            }

        };
    }
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                         .AllowAnyHeader()
                         .AllowAnyMethod());
});


WebApplication app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
