using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using QuizCraft.Domain.API.Controllers;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Tests.Unit.ControllersTests;

public class QuizControllerTests
{
    private readonly Mock<IQuizService> _quizService = new();
    private readonly Mock<IFileProcessingService> _fileProcessingService = new();
    private readonly Mock<IPdfExportService> _pdfExportService = new();
    private readonly QuizController _controller;

    public QuizControllerTests()
    {
        _controller = new QuizController(_quizService.Object, _fileProcessingService.Object, _pdfExportService.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public async Task CreateQuizAsync_ReturnsOkResult()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var content = "Hello World from a Fake File";
        var fileName = "test.txt";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;
        fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.Length).Returns(ms.Length);

        var token = "test-token";
        _controller.HttpContext.Request.Headers.Authorization = $"Bearer {token}";

        _fileProcessingService.Setup(x => x.ProcessFileAsync(It.IsAny<IFormFile>())).ReturnsAsync("processed content");
        _quizService.Setup(x => x.CreateQuizAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Guid.NewGuid());

        // Act
        var result = await _controller.CreateQuizAsync(fileMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<Guid>(okResult.Value);
    }

    [Fact]
    public void GetQuizzes_ReturnsOkResult()
    {
        // Arrange
        var token = "test-token";
        _controller.HttpContext.Request.Headers.Authorization = $"Bearer {token}";

        var quizzes = new List<QuizDto> { new() { Id = Guid.NewGuid(), Title = "Test Quiz", CreatedAt = DateTime.UtcNow, Category = Constants.Category.Art } };
        _quizService.Setup(x => x.RetrieveQuizzes(It.IsAny<string>())).Returns(quizzes);

        // Act
        var result = _controller.GetQuizzes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(quizzes, okResult.Value);
    }

    [Fact]
    public async Task GetDetailedQuizDto_ReturnsOkResult()
    {
        // Arrange
        var token = "test-token";
        _controller.HttpContext.Request.Headers.Authorization = $"Bearer {token}";

        var quizId = Guid.NewGuid();
        var detailedQuiz = new DetailedQuizDto 
        { 
            Id = quizId, 
            Title = "Detailed Quiz", 
            CurrentQuestionId = Guid.NewGuid(), 
            Questions = [] 
        };
        _quizService.Setup(x => x.RetrieveQuestions(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(detailedQuiz);

        // Act
        var result = await _controller.GetDetailedQuizDto(quizId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(detailedQuiz, okResult.Value);
    }

    [Fact]
    public void ValidateAnswer_ReturnsOkResult()
    {
        // Arrange
        var token = "test-token";
        _controller.HttpContext.Request.Headers.Authorization = $"Bearer {token}";

        var quizId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var answerId = Guid.NewGuid();
        var inputDto = new AnswerAttemptDto(Guid.NewGuid());
        var validatedAnswer = new ValidatedAnswerDto { SelectedAnswer = new(answerId), CorrectAnswer = new() { Id = answerId, Text = "Test"} };
        _quizService.Setup(x => x.ValidateAnswer(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<AnswerAttemptDto>())).Returns(validatedAnswer);

        // Act
        var result = _controller.ValidateAnswer(quizId, questionId, inputDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(validatedAnswer, okResult.Value);
    }

    [Fact]
    public async Task CompleteQuizAttempt_ReturnsOkResult()
    {
        // Arrange
        var token = "test-token";
        _controller.HttpContext.Request.Headers.Authorization = $"Bearer {token}";

        var quizId = Guid.NewGuid();
        _quizService.Setup(x => x.CompleteQuizAttempt(It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CompleteQuizAttempt(quizId);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteQuiz_ReturnsOkResult()
    {
        // Arrange
        var token = "test-token";
        _controller.HttpContext.Request.Headers.Authorization = $"Bearer {token}";

        var quizId = Guid.NewGuid();
        _quizService.Setup(x => x.DeleteQuiz(It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteQuiz(quizId);

        // Assert
        Assert.IsType<OkResult>(result);
    }
}
