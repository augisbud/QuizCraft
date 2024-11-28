using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using QuizCraft.Domain.API.APIClients;
using QuizCraft.Domain.API.Data;
using QuizCraft.Domain.API.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace QuizCraft.Domain.API.Tests.Integration.Fixtures;

public class ControllerTestsFixture
{
    public readonly WireMockServer WireMockServer;
    public readonly CustomWebApplicationFactory Factory;

    public ControllerTestsFixture()
    {
        WireMockServer = WireMockServer.Start(8080);

        Factory = new CustomWebApplicationFactory();

        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<QuizzesDbContext>();

        context.Database.EnsureDeleted();

        context.Database.EnsureCreated();
        context.Quizzes.AddRange(DataFixture.Quizzes);
        context.QuizAttempts.AddRange(DataFixture.QuizAttempts);

        context.SaveChanges();
    }
}