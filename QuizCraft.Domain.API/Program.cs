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
using System.IdentityModel.Tokens.Jwt;
using Serilog;

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
    c.OperationFilter<SwaggerPdfResponseOperationFilter>();
});

builder.Services.AddScoped<JwtSecurityTokenHandler>();
builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();

builder.Services.AddHealthChecks();

builder.Services.AddDbContext<QuizzesDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IQuizAttemptRepository, QuizAttemptRepository>();
builder.Services.AddScoped<IQuizAnswerAttemptRepository, QuizAnswerAttemptRepository>();

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
builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddScoped<IPdfExportService, PdfExportService>();


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.Console()
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();

public partial class Program { }
