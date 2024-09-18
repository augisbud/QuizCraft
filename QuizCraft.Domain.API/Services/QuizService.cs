using System.Text.Json;
using QuizCraft.Domain.API.APIClients;
using QuizCraft.Domain.API.Models;

namespace QuizCraft.Domain.API.Services;

public class QuizService(IGeminiAPIClient geminiAPIClient) : IQuizService
{
    public async Task<QuestionDto> GenerateQuiz()
    {
        var sampleOutput = new QuestionDto()
        {
            Text = "The question you came up with.",
            Answers = [
                new()
                {
                    Text = "Possible Answers for the given question."
                }
            ]
        };

        var prompt = @"You have to come up with a simple question and four possible answers.
            I will provided a sample json structure in which to return the question itself and the four possible answers.
            Respond with that json structure and that json structure only, only fill in the provided properties.
            The expected output structure is: " + JsonSerializer.Serialize(sampleOutput);

        var response = await geminiAPIClient.PostAsync(prompt);

        var data = JsonSerializer.Deserialize<QuestionDto>(response.Candidates[0].Content.Parts[0].Text);
        
        return data!;
    }
}