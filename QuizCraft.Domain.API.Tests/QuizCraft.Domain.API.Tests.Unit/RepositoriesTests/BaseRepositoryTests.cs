using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Profiles;
using QuizCraft.Domain.API.Repositories;

namespace QuizCraft.Domain.API.Tests.Unit.RepositoriesTests;

public class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
{
    public DbSet<QuizAttempt> QuizAttempts { get; set; } = null!;
    public DbSet<QuizAnswerAttempt> QuizAnswerAttempts { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;
}

public class BaseRepositoryTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly IMapper _mapper;
    private readonly BaseRepository<QuizAttempt> _repository;

    public BaseRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new TestDbContext(options);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<QuizzesProfiles>();
        });
        _mapper = config.CreateMapper();

        _repository = new BaseRepository<QuizAttempt>(_context, _mapper);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _context.Dispose();
    }

    [Fact]
    public async Task CreateAsync_AddsEntityAndSavesChanges()
    {
        // Arrange
        var entity = new QuizAttempt
        {
            QuizId = Guid.NewGuid(),
            UserEmail = "user@example.com",
            IsCompleted = false
        };

        // Act
        var result = await _repository.CreateAsync(entity);

        // Assert
        Assert.Equal(entity, result);
        var dbEntity = await _context.QuizAttempts.FindAsync(entity.Id);
        Assert.NotNull(dbEntity);
        Assert.Equal(entity.Id, dbEntity.Id);
    }

    [Fact]
    public async Task RetrieveAll_ReturnsAllEntities()
    {
        // Arrange
        var entities = new List<QuizAttempt>
        {
            new()
            {
                QuizId = Guid.NewGuid(),
                UserEmail = "user1@example.com",
                IsCompleted = false
            },
            new()
            {
                QuizId = Guid.NewGuid(),
                UserEmail = "user2@example.com",
                IsCompleted = true
            }
        };
        _context.QuizAttempts.AddRange(entities);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.RetrieveAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, e => e.Id == entities[0].Id);
        Assert.Contains(result, e => e.Id == entities[1].Id);
    }


    [Fact]
    public async Task RetrieveByIdAsync_ReturnsEntity()
    {
        // Arrange
        var entity = new QuizAttempt
        {
            QuizId = Guid.NewGuid(),
            UserEmail = "user@example.com",
            IsCompleted = false
        };
        _context.QuizAttempts.Add(entity);
        _context.SaveChanges();

        // Act
        var result = await _repository.RetrieveByIdAsync(entity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
    }

    [Fact]
    public void RetrieveByCondition_ReturnsFilteredEntities()
    {
        // Arrange
        var completedAttempt = new QuizAttempt
        {
            QuizId = Guid.NewGuid(),
            UserEmail = "user1@example.com",
            IsCompleted = true
        };
        var incompleteAttempt = new QuizAttempt
        {
            QuizId = Guid.NewGuid(),
            UserEmail = "user2@example.com",
            IsCompleted = false
        };
        _context.QuizAttempts.AddRange(completedAttempt, incompleteAttempt);
        _context.SaveChanges();

        // Act
        var result = _repository.RetrieveByCondition(a => a.IsCompleted);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, e => e.Id == completedAttempt.Id);
    }

    [Fact]
    public void Delete_RemovesEntityAndSavesChanges()
    {
        // Arrange
        var entity = new QuizAttempt
        {
            QuizId = Guid.NewGuid(),
            UserEmail = "user@example.com",
            IsCompleted = false
        };
        _context.QuizAttempts.Add(entity);
        _context.SaveChanges();

        // Act
        _repository.Delete(entity);

        // Assert
        var dbEntity = _context.QuizAttempts.Find(entity.Id);
        Assert.Null(dbEntity);
    }

    [Fact]
    public async Task SaveChangesAsync_ReturnsTrue_WhenChangesSaved()
    {
        // Arrange
        var entity = new QuizAttempt
        {
            QuizId = Guid.NewGuid(),
            UserEmail = "user@example.com",
            IsCompleted = false
        };
        _context.QuizAttempts.Add(entity);

        // Act
        var result = await _repository.SaveChangesAsync();

        // Assert
        Assert.True(result);
        var dbEntity = await _context.QuizAttempts.FindAsync(entity.Id);
        Assert.NotNull(dbEntity);
    }

    [Fact]
    public void RetrieveProjected_ReturnsProjectedEntities()
    {
        // Arrange
        var entities = new List<QuizAttempt>
            {
                new() {
                    QuizId = Guid.NewGuid(),
                    UserEmail = "user1@example.com",
                    IsCompleted = false
                },
                new() {
                    QuizId = Guid.NewGuid(),
                    UserEmail = "user2@example.com",
                    IsCompleted = true
                }
            };
        _context.QuizAttempts.AddRange(entities);
        _context.SaveChanges();

        // Act
        var result = _repository.RetrieveProjected<QuizAttempt, QuizAttemptDto>();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, e => e.Id == entities[0].Id);
        Assert.Contains(result, e => e.Id == entities[1].Id);
    }
}