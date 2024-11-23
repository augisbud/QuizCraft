using System.Text.Json;
using QuizCraft.Domain.API.Exceptions;

namespace QuizCraft.Domain.API.APIClients;

public interface IGeminiAPIClient
{
    Task<Output> PostAsync(string prompt);
}

public class GeminiAPIClient(IConfiguration configuration, HttpClient httpClient) : IGeminiAPIClient
{
    private readonly string APIKey = configuration.GetValue<string>("GeminiAPIKey") ?? throw new MissingConfigurationException("GeminiAPIKey");
    private readonly JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly HttpClient httpClient = httpClient;

    public async Task<Output> PostAsync(string prompt)
    {
        var input = new Input(
            [
                new Content(
                    [
                        new Part(prompt)
                    ],
                    "user"
                )
            ]
        );

        var response = await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}?key={APIKey}", input);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.StatusCode}).");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<Output>(jsonResponse, jsonSerializerOptions);

        if (result == null || result.Candidates.Count == 0)
        {
            throw new HttpRequestException("Received an empty or invalid response from the Gemini API.");
        }

        return result;
    }
}

public record Input(List<Content> Contents);
public record Output(List<Candidate> Candidates, UsageMetadata UsageMetadata);
public record Candidate(Content Content, string FinishReason, int? Index);
public record UsageMetadata(int PromptTokenCount, int CandidatesTokenCount, int TotalTokenCount);
public record Content(List<Part> Parts, string? Role);
public record Part(string Text);