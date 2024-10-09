using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using QuizCraft.Domain.API.APIClients;

namespace QuizCraft.Domain.API.Tests.Unit;

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
        var expectedOutput = new Output
        {
            Candidates = new List<Candidate>
            {
                new Candidate
                {
                    Content = new Content
                    {
                        Parts = new List<Part>
                        {
                            new Part { Text = "Response text" }
                        },
                        Role = null
                    },
                    FinishReason = "complete",
                    Index = 0,
                    SafetyRatings = new List<SafetyRating>
                    {
                        new SafetyRating { Category = "safe", Probability = "high" }
                    }
                }
            },
            UsageMetadata = new UsageMetadata
            {
                PromptTokenCount = 1,
                CandidatesTokenCount = 1,
                TotalTokenCount = 2
            }
        };

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
    public async Task PostAsync_InvalidResponse_ThrowsHttpRequestException()
    {
        // Arrange
        var prompt = "Test prompt";

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

        _httpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(() => _geminiAPIClient.PostAsync(prompt));
        Assert.Contains("Response status code does not indicate success", exception.Message);
    }
}
