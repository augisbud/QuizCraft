using Moq;
using QuizCraft.Domain.API.Exceptions;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Tests.Unit.ServicesTests;

public class PdfExportServiceTests
{
    private readonly Mock<IQuizService> _quizService = new();
    private readonly PdfExportService _pdfExportService;

    public PdfExportServiceTests()
    {
        _pdfExportService = new PdfExportService(_quizService.Object);
    }

    [Fact]
    public async Task GenerateQuizPdfAsync_ReturnsPdfBytes_WhenQuizExists()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var token = "testToken";
        var quizDetails = new DetailedQuizDto
        {
            Id = quizId,
            Title = "Sample Quiz",
            CurrentQuestionId = Guid.NewGuid(),
            Questions = new List<QuestionDto>
            {
                new QuestionDto { Id = Guid.NewGuid(), Text = "Question 1", Answers = new List<AnswerDto> { new AnswerDto { Id = Guid.NewGuid(), Text = "Answer 1" } } },
                new QuestionDto { Id = Guid.NewGuid(), Text = "Question 2", Answers = new List<AnswerDto> { new AnswerDto { Id = Guid.NewGuid(), Text = "Answer 2" } } }
            }
        };

        _quizService.Setup(x => x.RetrieveQuestions(quizId, token)).ReturnsAsync(quizDetails);

        // Act
        var result = await _pdfExportService.GenerateQuizPdfAsync(quizId, token);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Length > 0); // Ensure that the PDF bytes are returned
    }

    [Fact]
    public async Task GenerateQuizPdfAsync_ThrowsQuizNotFoundException_WhenQuizDoesNotExist()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var token = "testToken";

        _quizService.Setup(x => x.RetrieveQuestions(quizId, token)).ThrowsAsync(new QuizNotFoundException(quizId));

        // Act & Assert
        await Assert.ThrowsAsync<QuizNotFoundException>(() => _pdfExportService.GenerateQuizPdfAsync(quizId, token));
    }

    [Fact]
    public async Task GenerateQuizPdfAsync_ReturnsPdfBytes_WhenQuizHasNoQuestions()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var token = "testToken";
        var quizDetails = new DetailedQuizDto
        {
            Id = quizId,
            Title = "Empty Quiz",
            CurrentQuestionId = Guid.NewGuid(),
            Questions = new List<QuestionDto>() // No questions
        };

        _quizService.Setup(x => x.RetrieveQuestions(quizId, token)).ReturnsAsync(quizDetails);

        // Act
        var result = await _pdfExportService.GenerateQuizPdfAsync(quizId, token);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Length > 0); // Ensure that the PDF bytes are returned even if there are no questions
    }

    [Fact]
    public async Task GenerateQuizPdfAsync_ReturnsPdfBytes_WhenQuestionsHaveNoAnswers()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var token = "testToken";
        var quizDetails = new DetailedQuizDto
        {
            Id = quizId,
            Title = "Quiz with No Answers",
            CurrentQuestionId = Guid.NewGuid(),
            Questions = new List<QuestionDto>
            {
                new QuestionDto { Id = Guid.NewGuid(), Text = "Question 1", Answers = new List<AnswerDto>() } // No answers
            }
        };

        _quizService.Setup(x => x.RetrieveQuestions(quizId, token)).ReturnsAsync(quizDetails);

        // Act
        var result = await _pdfExportService.GenerateQuizPdfAsync(quizId, token);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Length > 0); // Ensure that the PDF bytes are returned even if questions have no answers
    }
} 