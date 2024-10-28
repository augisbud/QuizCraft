using QuizCraft.Domain.API.APIClients;
using QuizCraft.Domain.API.Services;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using QuizCraft.Domain.API.Data;
using QuizCraft.Domain.API.Repositories;
using QuizCraft.Domain.API.Profiles;
using AutoMapper.EquivalencyExpression;
using QuizCraft.Domain.API.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
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

builder.Services.AddAutoMapper(cfg => {
    cfg.AddProfile<QuizzesProfiles>();
    cfg.AddCollectionMappers();
});

builder.Services.AddScoped<IQuizService, QuizService>();

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuizCraft API v1"));

app.UseHttpsRedirection();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();

public partial class Program { }