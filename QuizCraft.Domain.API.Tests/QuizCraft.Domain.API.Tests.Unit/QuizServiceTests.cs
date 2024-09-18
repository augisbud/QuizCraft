using System.Text.Json;
using Moq;
using QuizCraft.Domain.API.APIClients;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Tests.Unit;

public class QuizServiceTests
{
    private readonly Mock<IGeminiAPIClient> _geminiAPIClient = new();
    private readonly QuizService _quizService;

    public QuizServiceTests()
    {
        _quizService = new QuizService(_geminiAPIClient.Object);
    }

    [Fact]
    public async void GenerateQuiz_ReturnsExpected()
    {
        // Arrange
        var expectedOutput = new QuestionDto()
        {
            Text = "What is the capital of France?",
            Answers = [
                new() { Text = "Paris" },
                new() { Text = "London" },
                new() { Text = "Berlin" },
                new() { Text = "Madrid" }
            ]
        };

        var responseContent = new Output()
        {
            Candidates = [
                new()
                {
                    Content = new()
                    {
                        Parts = [
                            new()
                            {
                                Text = JsonSerializer.Serialize(expectedOutput)
                            }
                        ],
                        Role = "prompt"
                    },
                    FinishReason = "length",
                    Index = 0,
                    SafetyRatings = [
                        new()
                        {
                            Category = "safe",
                            Probability = "1"
                        }
                    ]
                }
            ],
            UsageMetadata = new()
            {
                PromptTokenCount = 1,
                CandidatesTokenCount = 1,
                TotalTokenCount = 1
            }
        };

        _geminiAPIClient
            .Setup(x => x.PostAsync(It.IsAny<string>()))
            .ReturnsAsync(responseContent);

        // Act
        var result = await _quizService.GenerateQuiz();

        // Assert
        Assert.Equal(expectedOutput.Text, result.Text);
        foreach(var answer in expectedOutput.Answers)
        {
            Assert.Contains(result.Answers, x => x.Text == answer.Text);
        }
    }
}