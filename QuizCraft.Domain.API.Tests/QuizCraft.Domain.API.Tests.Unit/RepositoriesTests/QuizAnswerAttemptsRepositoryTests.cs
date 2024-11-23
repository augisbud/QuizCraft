using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizCraft.Domain.API.Data;
using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Profiles;
using QuizCraft.Domain.API.Repositories;

namespace QuizCraft.Domain.API.Tests.Unit.RepositoriesTests;

public class QuizAnswerAttemptRepositoryTests : IDisposable
{
    private readonly QuizzesDbContext _context;
    private readonly IMapper _mapper;
    private readonly QuizAnswerAttemptRepository _repository;

    public QuizAnswerAttemptRepositoryTests()
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

        _repository = new QuizAnswerAttemptRepository(_context, _mapper);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void RetrieveQuizAnswerAttempts_ReturnsCorrectAttempts()
    {
        // Arrange
        var attemptId = Guid.NewGuid();
        var otherAttemptId = Guid.NewGuid();

        var attempts = new List<QuizAnswerAttempt>
            {
                new() {
                    QuizAttemptId = attemptId,
                    QuestionId = Guid.NewGuid(),
                    AnswerId = Guid.NewGuid()
                },
                new() {
                    QuizAttemptId = attemptId,
                    QuestionId = Guid.NewGuid(),
                    AnswerId = Guid.NewGuid()
                },
                new() {
                    QuizAttemptId = otherAttemptId,
                    QuestionId = Guid.NewGuid(),
                    AnswerId = Guid.NewGuid()
                }
            };
        _context.QuizAnswerAttempts.AddRange(attempts);
        _context.SaveChanges();

        // Act
        var result = _repository.RetrieveQuizAnswerAttempts(attemptId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, a => Assert.Equal(attemptId, a.QuizAttemptId));
    }

    [Fact]
    public void CreateQuizAnswerAttempt_AddsEntityAndSavesChanges()
    {
        // Arrange
        var attemptId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var answerId = Guid.NewGuid();

        // Act
        var result = _repository.CreateQuizAnswerAttempt(attemptId, questionId, answerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(attemptId, result.QuizAttemptId);
        Assert.Equal(questionId, result.QuestionId);
        Assert.Equal(answerId, result.AnswerId);

        var dbEntity = _context.QuizAnswerAttempts.Find(result.Id);
        Assert.NotNull(dbEntity);
        Assert.Equal(result.Id, dbEntity.Id);
    }
}