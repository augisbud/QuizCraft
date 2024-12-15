using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Moq;
using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Exceptions;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Repositories;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Tests.Unit.ServicesTests;

public class StatisticsServiceTests
{
    private readonly Mock<IQuizRepository> _quizRepository = new();
    private readonly Mock<IQuizAttemptRepository> _quizAttemptRepository = new();
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly StatisticsService _statisticsService;

    public StatisticsServiceTests()
    {
        _statisticsService = new StatisticsService(_quizRepository.Object, _quizAttemptRepository.Object, _jwtSecurityTokenHandler, _mapper.Object);
    }

    [Fact]
    public async Task QuizAttemptsForUser_Throws_QuizNotFoundException()
    {
        // Arrange
        var token = _jwtSecurityTokenHandler.WriteToken(new JwtSecurityToken(claims: [new Claim("email", "test@test.com")]));
        var id = Guid.NewGuid();

        _quizRepository
            .Setup(x => x.RetrieveByIdAsync(id, It.IsAny<System.Linq.Expressions.Expression<Func<Entities.Quiz, object>>[]>()))
            .ReturnsAsync((Quiz)null!);

        // Act & Assert
        await Assert.ThrowsAsync<QuizNotFoundException>(() => _statisticsService.QuizAttemptsForUser(token, id));
    }

    [Fact]
    public async Task QuizAttemptsForUser_Returns_Expected()
    {
        // Arrange
        var token = _jwtSecurityTokenHandler.WriteToken(new JwtSecurityToken(claims: [new Claim("email", "test@test.com")]));
        var id = Guid.NewGuid();
        var now = DateTime.Now;

        _quizRepository
            .Setup(x => x.RetrieveByIdAsync(id, It.IsAny<System.Linq.Expressions.Expression<Func<Entities.Quiz, object>>[]>()))
            .ReturnsAsync(new Quiz()
            {
                Id = id,
                Title = "Test Quiz",
                Category = Constants.Category.Art,
                CreatedBy = "test@test.com",
                Questions =
                [
                    new Question()
                    {
                        QuizId = id,
                        Text = "Test Question",
                    }
                ]
            });

        _quizAttemptRepository
            .Setup(x => x.RetrieveQuizAttempts(id, "test@test.com"))
            .Returns(
                [
                    new QuizAttempt()
                    {
                        QuizId = id,
                        UserEmail = "test@test.com",
                        IsCompleted = true
                    }
                ]
            );

        _mapper
            .Setup(x => x.Map<List<QuizAttemptDto>>(It.IsAny<IEnumerable<QuizAttempt>>()))
            .Returns(
                [
                    new QuizAttemptDto()
                    {
                        Id = Guid.NewGuid(),
                        StartedAt = now,
                        CorrectAnswers = 1
                    }
                ]
            );


        // Act
        var data = await _statisticsService.QuizAttemptsForUser(token, id);

        // Assert
        Assert.Equal("Test Quiz", data.Name);
        Assert.Equal(1, data.Answers);
        Assert.Single(data.Attempts);
        Assert.Equal(1, data.Attempts.First().CorrectAnswers);
        Assert.Equal(now, data.Attempts.First().StartedAt);
    }

    [Fact]
    public async Task GlobalStatisticsAsync_Returns_Expected()
    {
        // Arrange
        var quizAttempts = new List<QuizAttempt>
        {
            new() 
            {
                Id = Guid.NewGuid(),
                QuizId = Guid.NewGuid(),
                UserEmail = "user1@test.com",
                IsCompleted = true,
                StartedAt = DateTime.UtcNow
            },
            new() 
            {
                Id = Guid.NewGuid(),
                QuizId = Guid.NewGuid(),
                UserEmail = "user2@test.com",
                IsCompleted = true,
                StartedAt = DateTime.UtcNow
            },
            new() 
            {
                Id = Guid.NewGuid(),
                QuizId = Guid.NewGuid(),
                UserEmail = "user1@test.com",
                IsCompleted = true,
                StartedAt = DateTime.UtcNow
            }
        };

        var quizzes = new List<Quiz>
        {
            new() 
            {
                Id = Guid.NewGuid(),
                Title = "Quiz 1",
                Category = Constants.Category.Science,
                CreatedBy = "creator@test.com"
            },
            new() 
            {
                Id = Guid.NewGuid(),
                Title = "Quiz 2",
                Category = Constants.Category.Art,
                CreatedBy = "creator@test.com"
            }
        };

        _quizAttemptRepository
            .Setup(x => x.RetrieveAllAsync())
            .ReturnsAsync(quizAttempts);

        _quizRepository
            .Setup(x => x.RetrieveAllAsync())
            .ReturnsAsync(quizzes);

        // Act
        var result = await _statisticsService.GlobalStatisticsAsync();

        // Assert
        Assert.Equal(2, result.TotalUsers);
        Assert.Equal(2, result.TotalQuizzesCreated);
        Assert.Equal(1.5, result.AverageQuizzesPerUser);
    }

    [Fact]
    public async Task GlobalStatisticsAsync_Returns_Zero_When_No_Data()
    {
        // Arrange
        _quizAttemptRepository
            .Setup(x => x.RetrieveAllAsync())
            .ReturnsAsync([]);

        _quizRepository
            .Setup(x => x.RetrieveAllAsync())
            .ReturnsAsync([]);

        // Act
        var result = await _statisticsService.GlobalStatisticsAsync();

        // Assert
        Assert.Equal(0, result.TotalUsers);
        Assert.Equal(0, result.TotalQuizzesCreated);
        Assert.Equal(0, result.AverageQuizzesPerUser);
    }
}