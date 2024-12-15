using WireMock.Server;

namespace QuizCraft.Domain.API.Tests.Integration.Fixtures;

public class IncorrectConfigurationTestsFixture
{
    public readonly WireMockServer WireMockServer;
    public readonly CustomWebApplicationFactory Factory;

    public IncorrectConfigurationTestsFixture()
    {
        WireMockServer = WireMockServer.Start(8081);

        Factory = new CustomWebApplicationFactory(
            [
                new KeyValuePair<string, string?>("Frontend:Url", "http://localhost:3000"),
            ]
        );
    }
}