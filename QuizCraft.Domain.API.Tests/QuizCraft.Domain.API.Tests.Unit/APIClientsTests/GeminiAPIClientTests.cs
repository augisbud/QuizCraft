using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using QuizCraft.Domain.API.APIClients;

namespace QuizCraft.Domain.API.Tests.Unit.APIClientsTests;

public class GeminiAPIClientTests
{
    private readonly IConfiguration _configuration;
    private readonly Mock<HttpMessageHandler> _httpMessageHandler = new();
    private readonly HttpClient _httpClient;
    private readonly GeminiAPIClient _geminiAPIClient;

    public GeminiAPIClientTests()
    {
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?> {
                    {"GeminiAPIKey", "fake-api-key"}
                }
            )
            .Build();

        _httpClient = new HttpClient(_httpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost:8080/geminiAPI")
        };

        _geminiAPIClient = new GeminiAPIClient(_configuration, _httpClient);
    }

    [Fact]
    public async Task PostAsync_ValidResponse_ReturnsOutput()
    {
        // Arrange
        var prompt = "Test prompt";
        var expectedOutput = new Output(
            [
                new Candidate(
                    new Content(
                        [
                            new Part("Response text")
                        ],
                        null
                    ),
                    "complete",
                    0
                )
            ],
            new UsageMetadata(1, 1, 2)
        );

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(expectedOutput)
        };

        _httpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act
        var result = await _geminiAPIClient.PostAsync(prompt);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedOutput.Candidates.Count, result.Candidates.Count);
        Assert.Equal(expectedOutput.UsageMetadata.TotalTokenCount, result.UsageMetadata.TotalTokenCount);
    }

    [Fact]
    public async Task PostAsync_InvalidResponse_ThrowsException()
    {
        // Arrange
        var prompt = "Test prompt";

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

        _httpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => _geminiAPIClient.PostAsync(prompt));
    }

    [Fact]
    public async Task PostAsync_EmptyOrInvalidResponse_ThrowsException()
    {
        // Arrange
        var prompt = "Test prompt";

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"Candidates\":[],\"UsageMetadata\":{\"PromptTokenCount\":1,\"CandidatesTokenCount\":1,\"TotalTokenCount\":2}}", System.Text.Encoding.UTF8, "application/json")
        };

        _httpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => _geminiAPIClient.PostAsync(prompt));
    }
}