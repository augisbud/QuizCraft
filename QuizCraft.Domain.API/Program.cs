using QuizCraft.Domain.API.APIClients;
using QuizCraft.Domain.API.Services;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using QuizCraft.Domain.API.Data;
using QuizCraft.Domain.API.Repositories;
using QuizCraft.Domain.API.Profiles;
using AutoMapper.EquivalencyExpression;
using QuizCraft.Domain.API.Filters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = "https://accounts.google.com";
        options.Audience = builder.Configuration["Authentication:Google:ClientId"]!;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = "https://accounts.google.com",
            ValidAudience = builder.Configuration["Authentication:Google:ClientId"]!,
        };
    });

builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("AllowFrontend",
        b =>
        {
            b.WithOrigins(builder.Configuration["Frontend:Url"]!)
                .AllowAnyMethod()
                .AllowAnyHeader();
        });

    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "QuizCraft API", Version = "v1" });
    c.OperationFilter<SwaggerFileUploadOperationFilter>();
});

builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();

builder.Services.AddHealthChecks();

builder.Services.AddDbContext<QuizzesDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IQuizRepository, QuizRepository>();

builder.Services.AddHttpClient<IGeminiAPIClient, GeminiAPIClient>(
    client =>
    {
        client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent");
    }
);

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<QuizzesProfiles>();
    cfg.AddCollectionMappers();
});

builder.Services.AddScoped<IQuizService, QuizService>();

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuizCraft API v1"));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/favicon.ico"))
    {
        context.Response.StatusCode = 204;
        return;
    }
    await next();
});

app.Run();

public partial class Program { }