using Microsoft.Extensions.DependencyInjection;
using QuizCraft.Domain.API.Data;
using WireMock.Server;

namespace QuizCraft.Domain.API.Tests.Integration.Fixtures;

public class ControllerTestsFixture
{
    public readonly WireMockServer WireMockServer;
    public readonly CustomWebApplicationFactory Factory;

    public ControllerTestsFixture()
    {
        WireMockServer = WireMockServer.Start(8080);

        Factory = new CustomWebApplicationFactory(
            [
                new KeyValuePair<string, string?>("GeminiAPIKey", "fake-api-key"),
                new KeyValuePair<string, string?>("Frontend:Url", "http://localhost:3000"),
                new KeyValuePair<string, string?>("Authentication:Google:ClientId", "12345"),
                new KeyValuePair<string, string?>("Authentication:Google:ClientSecret", "12345")
            ]
        );

        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<QuizzesDbContext>();

        context.Database.EnsureDeleted();

        context.Database.EnsureCreated();
        context.Quizzes.AddRange(DataFixture.Quizzes);
        context.QuizAttempts.AddRange(DataFixture.QuizAttempts);

        context.SaveChanges();
    }
}