using System.Text.Json;
using AutoMapper;
using Moq;
using QuizCraft.Domain.API.APIClients;
using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Repositories;
using QuizCraft.Domain.API.Services;
using Microsoft.AspNetCore.Http;
using System.Text;

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
    public async Task CreateQuizAsync_ReturnsExpected()
    {
        // Arrange
        var processedContent = "This is the processed content of the file.";

        var expectedQuestions = new List<Question>
        {
            new Question
            {
                Text = "What is the capital of France?",
                Answers = new List<Answer>
                {
                    new() { Text = "Paris" },
                    new() { Text = "London" },
                    new() { Text = "Berlin" },
                    new() { Text = "Madrid" }
                }
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
            Id = Guid.NewGuid(),
            Questions = expectedQuestions
        };

        var quizDto = new QuizDto
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Questions = expectedQuestions.Select(q => new QuestionDto
            {
                Text = q.Text,
                Answers = q.Answers.Select(a => new AnswerDto(a.Text, a.IsCorrect)).ToList()
            }).ToList()
        };

        _geminiAPIClient
            .Setup(x => x.PostAsync(It.IsAny<string>()))
            .ReturnsAsync(responseContent);

        _mapper
            .Setup(x => x.Map<List<Question>>(It.IsAny<List<QuestionDto>>()))
            .Returns(expectedQuiz.Questions);

        _quizRepository
            .Setup(x => x.CreateQuizAsync(It.IsAny<Quiz>()))
            .ReturnsAsync(expectedQuiz);

        _mapper
            .Setup(x => x.Map<QuizDto>(It.IsAny<Quiz>()))
            .Returns(quizDto);

        // Act
        var result = await _quizService.CreateQuizAsync(processedContent);

        // Assert
        Assert.Equal(expectedQuestions[0].Text, result.Questions[0].Text);
        foreach (var answer in expectedQuestions[0].Answers)
        {
            Assert.Contains(result.Questions[0].Answers, x => x.Text == answer.Text);
        }

        _geminiAPIClient.Verify(x => x.PostAsync(It.IsAny<string>()), Times.Once);
        _quizRepository.Verify(x => x.CreateQuizAsync(It.IsAny<Quiz>()), Times.Once);
        _mapper.Verify(x => x.Map<List<Question>>(It.IsAny<List<QuestionDto>>()), Times.Once);
        _mapper.Verify(x => x.Map<QuizDto>(It.IsAny<Quiz>()), Times.Once);
    }


    [Fact]
    public async Task RetrieveQuizzes_ReturnsExpected()
    {
        // Arrange
        var expectedQuizzes = new List<Quiz>
        {
            new Quiz
            {
                Id = Guid.NewGuid(),
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "What is the capital of France?",
                        Answers = new List<Answer>
                        {
                            new() { Text = "Paris" },
                            new() { Text = "London" },
                            new() { Text = "Berlin" },
                            new() { Text = "Madrid" }
                        }
                    }
                }
            }
        };

        _quizRepository
            .Setup(x => x.RetrieveQuizzes())
            .ReturnsAsync(expectedQuizzes);


        // Act
        var result = await _quizService.RetrieveQuizzes();
        Assert.NotNull(result);
        Assert.True(result.Any(), "No quizzes were returned.");

        // Assert
        Assert.Equal(expectedQuizzes.Count, result.Count());

        for (int i = 0; i < expectedQuizzes.Count; i++)
        {
            var expectedQuiz = expectedQuizzes[i];
            var actualQuiz = result.ElementAt(i);

            Assert.Equal(expectedQuiz.Questions.Count, actualQuiz.Questions.Count);

            for (int j = 0; j < expectedQuiz.Questions.Count; j++)
            {
                Assert.Equal(expectedQuiz.Questions[j].Text, actualQuiz.Questions[j].Text);
            }
        }

        _quizRepository.Verify(x => x.RetrieveQuizzes(), Times.Once);
    }

}