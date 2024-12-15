using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Moq;
using QuizCraft.Domain.API.APIClients;
using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Exceptions;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Repositories;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Tests.Unit.ServicesTests;

public class QuizServiceTests
{
    private readonly Mock<IGeminiAPIClient> _geminiAPIClient = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IQuizRepository> _quizRepository = new();
    private readonly Mock<IQuizAttemptRepository> _quizAttemptRepository = new();
    private readonly Mock<IQuizAnswerAttemptRepository> _quizAnswerAttemptRepository = new();
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();
    private readonly QuizService _quizService;

    public QuizServiceTests()
    {
        _quizService = new QuizService(
            _geminiAPIClient.Object,
            _mapper.Object,
            _quizRepository.Object,
            _quizAttemptRepository.Object,
            _quizAnswerAttemptRepository.Object,
            _jwtSecurityTokenHandler
        );
    }

    [Fact]
    public async Task CreateQuizAsync_CreatesQuizSuccessfully()
    {
        // Arrange
        var source = "Quiz Source Content";
        var token = GenerateToken("test@example.com");
        var geminiResponse = new Output(
            [
            new Candidate(
                new Content(
                    [
                        new Part("{\"Title\":\"Sample Quiz\",\"Category\":\"Art\",\"Questions\":[]}")
                    ],
                    null
                ),
                null!,
                null
            )
            ],
            new UsageMetadata(0, 0, 0)
        );

        var quizDtoForCreation = new QuizDtoForCreation
        {
            Title = "Sample Quiz",
            Category = Constants.Category.Art,
            Questions = new List<QuestionForCreationDto>()
        };

        var quiz = new Quiz
        {
            Id = Guid.NewGuid(),
            Category = Constants.Category.Art,
            Title = "Sample Quiz",
            Questions = [],
            CreatedBy = "test@example.com"
        };

        _geminiAPIClient.Setup(x => x.PostAsync(It.IsAny<string>())).ReturnsAsync(geminiResponse);
        _mapper.Setup(x => x.Map<Quiz>(It.IsAny<QuizDtoForCreation>())).Returns(quiz);
        _quizRepository.Setup(x => x.CreateAsync(It.IsAny<Quiz>())).ReturnsAsync(quiz);

        // Act
        var result = await _quizService.CreateQuizAsync(source, token);

        // Assert
        Assert.Equal(quiz.Id, result);
        _geminiAPIClient.Verify(x => x.PostAsync(It.IsAny<string>()), Times.Once);
        _mapper.Verify(x => x.Map<Quiz>(It.IsAny<QuizDtoForCreation>()), Times.Once);
        _quizRepository.Verify(x => x.CreateAsync(It.IsAny<Quiz>()), Times.Once);
    }

    [Fact]
    public async Task CreateQuizAsync_ThrowsInsufficientDataException()
    {
        // Arrange
        var source = "Invalid Source Content";
        var token = GenerateToken("test@example.com");
        var geminiResponse = new Output(
            [
                new Candidate(
                    new Content(
                        [
                            new Part("null")
                        ],
                        null
                    ),
                    null!,
                    null
                )
            ],
            new UsageMetadata(0, 0, 0)
        );

        _geminiAPIClient.Setup(x => x.PostAsync(It.IsAny<string>())).ReturnsAsync(geminiResponse);

        // Act & Assert
        await Assert.ThrowsAsync<InsufficientDataException>(() => _quizService.CreateQuizAsync(source, token));
    }

    [Fact]
    public void RetrieveQuizzes_ReturnsQuizzes_WhenTokenIsNull()
    {
        // Arrange
        var quizzes = new List<QuizForTransferDto>
        {
            new() { Id = Guid.NewGuid(), Title = "Quiz 1", Category = Constants.Category.Art, CreatedBy = "user1@example.com", CreatedAt = DateTime.Now },
            new() { Id = Guid.NewGuid(), Title = "Quiz 2", Category = Constants.Category.Art, CreatedBy = "user2@example.com", CreatedAt = DateTime.Now }
        };

        _quizRepository.Setup(x => x.RetrieveProjected<Quiz, QuizForTransferDto>()).Returns(quizzes.AsQueryable());

        // Act
        var result = _quizService.RetrieveQuizzes(null);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, quiz => Assert.False(quiz.IsOwner));
    }

    [Fact]
    public void RetrieveQuizzes_SetsIsOwner_WhenTokenIsProvided()
    {
        // Arrange
        var token = GenerateToken("user1@example.com");

        var quizzes = new List<QuizForTransferDto>
        {
            new() { Id = Guid.NewGuid(), Title = "Quiz 1", Category = Constants.Category.Art, CreatedBy = "user1@example.com", CreatedAt = DateTime.Now },
            new() { Id = Guid.NewGuid(), Title = "Quiz 2", Category = Constants.Category.Art, CreatedBy = "user2@example.com", CreatedAt = DateTime.Now }
        };

        _quizRepository.Setup(x => x.RetrieveProjected<Quiz, QuizForTransferDto>()).Returns(quizzes.AsQueryable());

        // Act
        var result = _quizService.RetrieveQuizzes(token).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.True(result[0].IsOwner);
        Assert.False(result[1].IsOwner);
    }

    [Fact]
    public async Task RetrieveQuestions_ReturnsDetailedQuizDto()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var token = GenerateToken("test@example.com");
        var quiz = new Quiz
        {
            Id = quizId,
            Title = "Sample Quiz",
            Category = Constants.Category.Art,
            Questions =
            [
                new Question { Id = Guid.NewGuid(), QuizId = quizId, Text = "Question 1" },
                new Question { Id = Guid.NewGuid(), QuizId = quizId, Text = "Question 2" }
            ],
            QuizAttempts = [],
            CreatedBy = "test@example.com"
        };

        _quizRepository.Setup(x => x.RetrieveQuizWithQuestionsById(quizId)).Returns(quiz);
        _mapper.Setup(x => x.Map<IEnumerable<QuestionDto>>(It.IsAny<IEnumerable<Question>>())).Returns(new List<QuestionDto>());

        // Act
        var result = await _quizService.RetrieveQuestions(quizId, token);

        // Assert
        Assert.Equal(quizId, result.Id);
        Assert.Equal("Sample Quiz", result.Title);
        _mapper.Verify(x => x.Map<IEnumerable<QuestionDto>>(It.IsAny<IEnumerable<Question>>()), Times.Once);
    }

    [Fact]
    public async Task RetrieveQuestions_ThrowsQuizNotFoundException()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var token = GenerateToken("test@example.com");

        _quizRepository.Setup(x => x.RetrieveQuizWithQuestionsById(quizId)).Returns((Quiz)null!);

        // Act & Assert
        await Assert.ThrowsAsync<QuizNotFoundException>(() => _quizService.RetrieveQuestions(quizId, token));
    }

    [Fact]
    public void ValidateAnswer_ReturnsValidatedAnswerDto()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var token = GenerateToken("test@example.com");
        var answerAttemptDto = new AnswerAttemptDto(Guid.NewGuid());
        var answerDto = new AnswerDto { Id = Guid.NewGuid(), Text = "Answer" };

        _quizRepository.Setup(x => x.RetrieveAnswer(quizId, questionId)).Returns(answerDto);
        _quizAttemptRepository.Setup(x => x.RetrieveQuizAttempt(quizId, "test@example.com")).Returns(new QuizAttempt { Id = Guid.NewGuid(), QuizId = quizId, UserEmail = "test@example.com", IsCompleted = false });

        // Act
        var result = _quizService.ValidateAnswer(token, quizId, questionId, answerAttemptDto);

        // Assert
        Assert.Equal(answerAttemptDto, result.SelectedAnswer);
        Assert.Equal(answerDto, result.CorrectAnswer);
        _quizAnswerAttemptRepository.Verify(x => x.CreateQuizAnswerAttempt(It.IsAny<Guid>(), questionId, answerAttemptDto.AnswerId), Times.Once);
    }

    [Fact]
    public void ValidateAnswer_ThrowsAnswerNotFoundException()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var token = GenerateToken("test@example.com");
        var answerAttemptDto = new AnswerAttemptDto(Guid.NewGuid());

        _quizRepository.Setup(x => x.RetrieveAnswer(quizId, questionId)).Returns((AnswerDto)null!);

        // Act & Assert
        Assert.Throws<AnswerNotFoundException>(() => _quizService.ValidateAnswer(token, quizId, questionId, answerAttemptDto));
    }

    [Fact]
    public async Task DeleteQuiz_DeletesQuizSuccessfully()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var token = GenerateToken("test@example.com");
        var quiz = new Quiz
        {
            Id = quizId,
            Title = "Test",
            Category = Constants.Category.Art,
            CreatedBy = "test@example.com"
        };

        _quizRepository.Setup(x => x.RetrieveByIdAsync(quizId)).ReturnsAsync(quiz);

        // Act
        await _quizService.DeleteQuiz(token, quizId);

        // Assert
        _quizRepository.Verify(x => x.Delete(quiz), Times.Once);
        _quizRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteQuiz_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var token = GenerateToken("user@example.com");
        var quiz = new Quiz
        {
            Id = quizId,
            Title = "Test",
            Category = Constants.Category.Art,
            CreatedBy = "owner@example.com"
        };

        _quizRepository.Setup(x => x.RetrieveByIdAsync(quizId)).ReturnsAsync(quiz);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _quizService.DeleteQuiz(token, quizId));
    }

    [Fact]
    public async Task RetrieveQuestions_HandlesExistingQuizAttempt()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var token = GenerateToken("test@example.com");
        var quiz = new Quiz
        {
            Id = quizId,
            Title = "Test Quiz",
            Category = Constants.Category.Art,
            CreatedBy = "test@example.com",
            Questions =
            [
                new() {
                    Id = Guid.NewGuid(),
                    Text = "Test Question 1",
                    QuizId = quizId
                },
                new() {
                    Id = Guid.NewGuid(),
                    Text = "Test Question 2",
                    QuizId = quizId
                }
            ]
        };
        quiz.QuizAttempts =
        [
            new QuizAttempt
            {
                QuizId = quizId,
                UserEmail = "test@example.com",
                IsCompleted = false,
                QuizAnswerAttempts =
                [
                    new QuizAnswerAttempt { QuestionId = quiz.Questions.First().Id, QuizAttemptId = quizId, AnswerId = Guid.NewGuid() }
                ]
            }
        ];

        _quizRepository.Setup(x => x.RetrieveQuizWithQuestionsById(quizId)).Returns(quiz);

        // Act
        var result = await _quizService.RetrieveQuestions(quizId, token);

        // Assert
        Assert.NotEqual(Guid.Empty, result.CurrentQuestionId);
    }

    [Fact]
    public async Task RetrieveQuestions_CompletesQuizAttemptWhenAllQuestionsAnswered()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var token = GenerateToken("test@example.com");
        var quiz = new Quiz
        {
            Id = quizId,
            Title = "Test Quiz",
            Category = Constants.Category.Art,
            CreatedBy = "test@example.com",
            Questions =
            [
                new() { Id = Guid.NewGuid(), Text = "Test Question", QuizId = quizId }
            ]
        };
        quiz.QuizAttempts =
        [
            new QuizAttempt
            {
                QuizId = quizId,
                UserEmail = "test@example.com",
                IsCompleted = false,
                QuizAnswerAttempts =
                [
                    new QuizAnswerAttempt { QuestionId = quiz.Questions.First().Id, QuizAttemptId = quizId, AnswerId = Guid.NewGuid() }
                ]
            }
        ];

        _quizRepository.Setup(x => x.RetrieveQuizWithQuestionsById(quizId)).Returns(quiz);
        _quizAttemptRepository.Setup(x => x.RetrieveQuizAttempt(quizId, "test@example.com")).Returns(quiz.QuizAttempts.First());

        // Act
        var result = await _quizService.RetrieveQuestions(quizId, token);

        // Assert
        _quizAttemptRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CompleteQuizAttempt_MarksAttemptAsCompleted()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var token = GenerateToken("test@example.com");
        var quizAttempt = new QuizAttempt
        {
            QuizId = quizId,
            UserEmail = "test@example.com",
            IsCompleted = false
        };

        _quizAttemptRepository.Setup(x => x.RetrieveQuizAttempt(quizId, "test@example.com")).Returns(quizAttempt);

        // Act
        await _quizService.CompleteQuizAttempt(token, quizId);

        // Assert
        Assert.True(quizAttempt.IsCompleted);
        _quizAttemptRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task RetrieveQuestions_ThrowsQuestionsNotFoundException_WhenNoQuestionsExist()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var token = GenerateToken("test@example.com");
        var quiz = new Quiz
        {
            Id = quizId,
            Title = "Empty Quiz",
            Category = Constants.Category.Art,
            Questions = new List<Question>(), // No questions
            QuizAttempts = new List<QuizAttempt>(),
            CreatedBy = "test@example.com"
        };

        _quizRepository.Setup(x => x.RetrieveQuizWithQuestionsById(quizId)).Returns(quiz);

        // Act & Assert
        await Assert.ThrowsAsync<QuestionsNotFoundException>(() => _quizService.RetrieveQuestions(quizId, token));
    }

    private string GenerateToken(string email)
    {
        var token = _jwtSecurityTokenHandler.WriteToken(new JwtSecurityToken(claims: new[]
        {
            new Claim("email", email)
        }));
        return token;
    }
}