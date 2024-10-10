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
    public async Task CreateQuiz_ReturnsExpected()
    {
        // TODO: Implement proper verification instead of using It.IsAny<Quiz>();

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

        var quizDto = new QuizDto()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Questions = expectedQuestions
        };

        _geminiAPIClient
            .Setup(x => x.PostAsync(It.Is<string>(x => x.Contains("'cities'"))))
            .ReturnsAsync(responseContent);

        _mapper
            .Setup(x => x.Map<List<Question>>(expectedQuestions))
            .Returns(expectedQuiz.Questions);

        _quizRepository
            .Setup(x => x.CreateQuizAsync(It.IsAny<Quiz>()))
            .ReturnsAsync(expectedQuiz);

        _mapper
            .Setup(x => x.Map<QuizDto>(It.IsAny<Quiz>()))
            .Returns(quizDto);

        // Act
        var result = await _quizService.CreateQuiz("cities");

        // Assert
        Assert.Equal(expectedQuestions[0].Text, result.Questions[0].Text);
        foreach (var answer in expectedQuestions[0].Answers)
        {
            Assert.Contains(result.Questions[0].Answers, x => x.Text == answer.Text);
        }

        _geminiAPIClient.Verify(x => x.PostAsync(It.Is<string>(x => x.Contains("'cities'"))), Times.Once);
        _quizRepository.Verify(x => x.CreateQuizAsync(It.IsAny<Quiz>()), Times.Once);
        _mapper.Verify(x => x.Map<List<Question>>(It.IsAny<List<QuestionDto>>()), Times.Once);
        _mapper.Verify(x => x.Map<QuizDto>(It.IsAny<Quiz>()), Times.Once);
    }

    [Fact]
    public void RetrieveQuizzes_ReturnsExpected()
    {
        // Arrange
        var expectedQuizzes = new List<QuizDto>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Questions =
                [
                    new QuestionDto()
                    {
                        Text = "What is the capital of France?",
                        Answers =
                        [
                            new AnswerDto() { Text = "Paris" },
                            new AnswerDto() { Text = "London" },
                            new AnswerDto() { Text = "Berlin" },
                            new AnswerDto() { Text = "Madrid" }
                        ]
                    }
                ]
            }
        };

        _quizRepository
            .Setup(x => x.RetrieveQuizzes())
            .Returns(expectedQuizzes);
        // Act
        var result = _quizService.RetrieveQuizzes();

        // Assert
        Assert.Equal(expectedQuizzes, result);

        _quizRepository.Verify(x => x.RetrieveQuizzes(), Times.Once);
    }
}