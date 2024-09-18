using System.Text.Json;
using QuizCraft.Domain.API.APIClients;
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

        WireMockServer
            .Given(Request.Create().WithPath("/geminiAPI").UsingPost())
            .RespondWith(Response.Create().WithStatusCode(200).WithBodyAsJson(
                new Output
                {
                    Candidates =
                    [
                        new Candidate
                        {
                            Content = new Content
                            {
                                Parts =
                                [
                                    new Part { Text = JsonSerializer.Serialize(new QuestionDto() { Text = "What is the capital of France?", Answers = [ new() { Text = "Paris" }, new() { Text = "Madrid" }]}) }
                                ],
                                Role = "prompt"
                            },
                            FinishReason = "complete",
                            Index = 0,
                            SafetyRatings =
                            [
                                new SafetyRating { Category = "safe", Probability = "high" }
                            ]
                        }
                    ],
                    UsageMetadata = new UsageMetadata
                    {
                        PromptTokenCount = 1,
                        CandidatesTokenCount = 1,
                        TotalTokenCount = 2
                    }
                }
            ));

        Factory = new CustomWebApplicationFactory();
    }
}