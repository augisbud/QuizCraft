using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizCraft.Domain.API.Data;
using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Profiles;
using QuizCraft.Domain.API.Repositories;

namespace QuizCraft.Domain.API.Tests.Unit.RepositoriesTests;

public class QuizRepositoryTests : IDisposable
{
    private readonly QuizzesDbContext _context;
    private readonly IMapper _mapper;
    private readonly QuizRepository _repository;

    public QuizRepositoryTests()
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

        _repository = new QuizRepository(_context, _mapper);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void RetrieveQuizWithQuestionsById_ReturnsCorrectQuiz()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var firstQuestionId = Guid.NewGuid();
        var secondQuestionId = Guid.NewGuid();
        var quiz = new Quiz
        {
            Id = quizId,
            Title = "Sample Quiz",
            Category = Constants.Category.Art,
            CreatedBy = "tester@example.com",
            Questions =
                [
                    new Question
                    {
                        Id = firstQuestionId,
                        QuizId = quizId,
                        Text = "Question 1",
                        Answers =
                        [
                            new Answer { QuestionId = firstQuestionId, Text = "Answer 1", IsCorrect = true },
                            new Answer { QuestionId = firstQuestionId, Text = "Answer 2", IsCorrect = false }
                        ]
                    },
                    new Question
                    {
                        Id = secondQuestionId,
                        QuizId = quizId,
                        Text = "Question 2",
                        Answers =
                        [
                            new Answer { QuestionId = secondQuestionId, Text = "Answer A", IsCorrect = false },
                            new Answer { QuestionId = secondQuestionId, Text = "Answer B", IsCorrect = true }
                        ]
                    }
                ]
        };
        _context.Quizzes.Add(quiz);
        _context.SaveChanges();

        // Act
        var result = _repository.RetrieveQuizWithQuestionsById(quizId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(quizId, result.Id);
        Assert.Equal("Sample Quiz", result.Title);
        Assert.Equal(2, result.Questions.Count);
        foreach (var question in result.Questions)
        {
            Assert.NotEmpty(question.Answers);
        }
    }

    [Fact]
    public void RetrieveQuestions_ReturnsCorrectQuestions()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var questions = new List<Question>
            {
                new() { Text = "What is 2+2?", QuizId = quizId },
                new() { Text = "What is the capital of France?", QuizId = quizId }
            };
        _context.Questions.AddRange(questions);
        _context.SaveChanges();

        // Act
        var result = _repository.RetrieveQuestions(quizId).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, q => q.Text == "What is 2+2?");
        Assert.Contains(result, q => q.Text == "What is the capital of France?");
    }

    [Fact]
    public void RetrieveAnswer_ReturnsCorrectAnswer()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var questionId = Guid.NewGuid();

        var question = new Question
        {
            Id = questionId,
            Text = "What is 2+2?",
            QuizId = quizId
        };

        var correctAnswer = new Answer
        {
            Text = "4",
            IsCorrect = true,
            QuestionId = questionId
        };
        var incorrectAnswer = new Answer
        {
            Text = "5",
            IsCorrect = false,
            QuestionId = questionId
        };

        _context.Questions.Add(question);
        _context.Answers.AddRange(correctAnswer, incorrectAnswer);
        _context.SaveChanges();

        // Act
        var result = _repository.RetrieveAnswer(quizId, questionId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("4", result.Text);
    }
}