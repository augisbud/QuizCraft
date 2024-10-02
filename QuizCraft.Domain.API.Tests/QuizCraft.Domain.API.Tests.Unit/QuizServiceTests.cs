using System.Text.Json;
using AutoMapper;
using Moq;
using QuizCraft.Domain.API.APIClients;
using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Repositories;
using QuizCraft.Domain.API.Services;
using Xunit;

namespace QuizCraft.Domain.API.Tests.Unit;

public class QuizServiceTests
{
    private readonly Mock<IGeminiAPIClient> _geminiAPIClient = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IQuizRepository> _quizRepository = new();
    private readonly QuizService _quizService;

    public QuizServiceTests()
    {
        _quizService = new QuizService(_geminiAPIClient.Object, _mapper.Object, _quizRepository.Object);
    }

    [Fact]
    public async void GenerateQuiz_ReturnsExpected()
    {
        // Arrange
        var expectedQuestions = new List<QuestionDto>
        {
            new()
            {
                Text = "What is the capital of France?",
                Answers =
                [
                    new() { Text = "Paris" },
                    new() { Text = "London" },
                    new() { Text = "Berlin" },
                    new() { Text = "Madrid" }
                ]
            }
        };

        var responseContent = new Output
        {
            Candidates =
            [
                new()
                {
                    Content = new Content
                    {
                        Parts =
                        [
                            new()
                            {
                                Text = JsonSerializer.Serialize(expectedQuestions)
                            }
                        ],
                        Role = "prompt"
                    },
                    FinishReason = "length",
                    Index = 0,
                    SafetyRatings =
                    [
                        new()
                        {
                            Category = "safe",
                            Probability = "1"
                        }
                    ]
                }
            ],
            UsageMetadata = new UsageMetadata
            {
                PromptTokenCount = 1,
                CandidatesTokenCount = 1,
                TotalTokenCount = 1
            }
        };

        var expectedQuiz = new Quiz
        {
            Questions =
            [
                new()
                {
                    Text = "What is the capital of France?",
                    Answers =
                    [
                        new() { Text = "Paris" },
                        new() { Text = "London" },
                        new() { Text = "Berlin" },
                        new() { Text = "Madrid" }
                    ]
                }
            ]
        };

        _geminiAPIClient
            .Setup(x => x.PostAsync(It.IsAny<string>()))
            .ReturnsAsync(responseContent);

        _quizRepository
            .Setup(x => x.AddQuizAsync(It.IsAny<Quiz>()))
            .ReturnsAsync(expectedQuiz);

        _mapper
            .Setup(x => x.Map<List<Question>>(It.IsAny<List<QuestionDto>>()))
            .Returns(expectedQuiz.Questions);

        _mapper
            .Setup(x => x.Map<QuizDto>(It.IsAny<Quiz>()))
            .Returns(new QuizDto()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Questions = expectedQuestions
            });

        // Act
        var result = await _quizService.CreateQuiz("cities");

        // Assert
        Assert.Equal(expectedQuestions[0].Text, result.Questions[0].Text);
        foreach (var answer in expectedQuestions[0].Answers)
        {
            Assert.Contains(result.Questions[0].Answers, x => x.Text == answer.Text);
        }
    }
}