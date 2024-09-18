using System.Text.Json;

namespace QuizCraft.Domain.API.APIClients;

public interface IGeminiAPIClient
{
    Task<Output> PostAsync(string prompt);
}

public class GeminiAPIClient(IConfiguration configuration, HttpClient httpClient) : IGeminiAPIClient
{   
    private readonly string APIKey = configuration.GetValue<string>("GeminiAPIKey") ?? throw new NotImplementedException(); // Ideally, we create our own exception class to inform the user about invalid configuration.
    private readonly JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<Output> PostAsync(string prompt)
    {
        var input = new Input()
        {
            Contents = [
                new()
                {
                    Parts = [
                        new()
                        {
                            Text = prompt
                        }
                    ],
                    Role = null
                }
            ]
        };

        var response = await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}?key={APIKey}", input);
        response.EnsureSuccessStatusCode();

        var result = JsonSerializer.Deserialize<Output>(await response.Content.ReadAsStringAsync(), jsonSerializerOptions);
    
        return result!;
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
    public required int Index { get; set; }
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

public class Part
{
    public required string Text { get; set; }
}