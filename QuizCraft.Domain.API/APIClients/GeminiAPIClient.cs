using System.Text.Json;

namespace QuizCraft.Domain.API.APIClients;

public interface IGeminiAPIClient
{
    Task<Output> PostAsync(string prompt);
}

public class GeminiAPIClient(IConfiguration configuration, HttpClient httpClient) : IGeminiAPIClient
{   
    private readonly string APIKey = configuration.GetValue<string>("GeminiAPIKey") ?? throw new NotImplementedException();
    private readonly JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly HttpClient httpClient = httpClient;

    public async Task<Output> PostAsync(string prompt)
    {
        var input = new Input()
        {
            Contents =
            [
                new()
                {
                    Parts =
                    [
                        new(prompt)
                    ],
                    Role = "user" // default role
                }
            ]
        };

        try
        {
            var response = await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}?key={APIKey}", input);

            // Check if the response indicates failure and throw HttpRequestException
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.StatusCode}).");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            //Console.WriteLine($"Gemini API Response: {jsonResponse}");

            var result = JsonSerializer.Deserialize<Output>(jsonResponse, jsonSerializerOptions);

            if (result == null || result.Candidates.Count == 0)
            {
                throw new Exception("Received an empty or invalid response from the Gemini API.");
            }

            return result;
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("Error while calling Gemini API: " + ex.Message, ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred: " + ex.Message, ex);
        }
    }
}

public class Input
{
    public required List<Content> Contents { get; set; }
}

public class Output
{
    public required List<Candidate> Candidates { get; set; }
    public required UsageMetadata UsageMetadata { get; set; }
}

public class Candidate
{
    public required Content Content { get; set; }
    public required string FinishReason { get; set; }
    public int? Index { get; set; }
    public required List<SafetyRating> SafetyRatings { get; set; }
}

public class UsageMetadata
{
    public required int PromptTokenCount { get; set; }
    public required int CandidatesTokenCount { get; set; }
    public required int TotalTokenCount { get; set; }
}

public class Content
{
    public required List<Part> Parts { get; set; }
    public required string? Role { get; set; }
}

public class SafetyRating
{
    public required string Category { get; set; }
    public required string Probability { get; set; }
}

// 2. Creating and using your own struct
public struct Part(string text)
{
    public string Text { get; set; } = text;
}