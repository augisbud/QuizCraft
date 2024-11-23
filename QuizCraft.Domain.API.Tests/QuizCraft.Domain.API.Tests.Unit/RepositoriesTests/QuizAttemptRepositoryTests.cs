using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizCraft.Domain.API.Data;
using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Profiles;
using QuizCraft.Domain.API.Repositories;

namespace QuizCraft.Domain.API.Tests.Unit.RepositoriesTests;

public class QuizAttemptRepositoryTests : IDisposable
{
    private readonly QuizzesDbContext _context;
    private readonly IMapper _mapper;
    private readonly QuizAttemptRepository _repository;

    public QuizAttemptRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<QuizzesDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new QuizzesDbContext(options);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<QuizzesProfiles>();
        });
        _mapper = config.CreateMapper();

        _repository = new QuizAttemptRepository(_context, _mapper);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void RetrieveQuizAttempt_ReturnsCorrectAttempt()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var email = "test@example.com";
        var otherQuizId = Guid.NewGuid();
        var attempts = new List<QuizAttempt>
            {
                new() {
                    QuizId = quizId,
                    UserEmail = email,
                    IsCompleted = false
                },
                new() {
                    QuizId = quizId,
                    UserEmail = email,
                    IsCompleted = true
                },
                new() {
                    QuizId = otherQuizId,
                    UserEmail = email,
                    IsCompleted = false
                }
            };
        _context.QuizAttempts.AddRange(attempts);
        _context.SaveChanges();

        // Act
        var result = _repository.RetrieveQuizAttempt(quizId, email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(quizId, result.QuizId);
        Assert.Equal(email, result.UserEmail);
        Assert.False(result.IsCompleted);
    }

    [Fact]
    public void RetrieveQuizAttempts_ReturnsAllAttempts()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var email = "test@example.com";
        var otherQuizId = Guid.NewGuid();
        var attempts = new List<QuizAttempt>
            {
                new() {
                    QuizId = quizId,
                    UserEmail = email,
                    IsCompleted = false,
                    StartedAt = DateTime.UtcNow.AddMinutes(-10)
                },
                new() {
                    QuizId = quizId,
                    UserEmail = email,
                    IsCompleted = true,
                    StartedAt = DateTime.UtcNow.AddMinutes(-5)
                },
                new() {
                    QuizId = otherQuizId,
                    UserEmail = email,
                    IsCompleted = false,
                    StartedAt = DateTime.UtcNow
                }
            };
        _context.QuizAttempts.AddRange(attempts);
        _context.SaveChanges();

        // Act
        var result = _repository.RetrieveQuizAttempts(quizId, email).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, a => Assert.Equal(quizId, a.QuizId));
        Assert.Equal(result.OrderBy(a => a.StartedAt), result);
    }

    [Fact]
    public void CreateQuizAttempt_AddsEntityAndSavesChanges()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var email = "newuser@example.com";

        // Act
        var result = _repository.CreateQuizAttempt(quizId, email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(quizId, result.QuizId);
        Assert.Equal(email, result.UserEmail);
        Assert.False(result.IsCompleted);

        var dbEntity = _context.QuizAttempts.Find(result.Id);
        Assert.NotNull(dbEntity);
        Assert.Equal(result.Id, dbEntity.Id);
        Assert.Equal(quizId, dbEntity.QuizId);
        Assert.Equal(email, dbEntity.UserEmail);
        Assert.False(dbEntity.IsCompleted);
    }
}
