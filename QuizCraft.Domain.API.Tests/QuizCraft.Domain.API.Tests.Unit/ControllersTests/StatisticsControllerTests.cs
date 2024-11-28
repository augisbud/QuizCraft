using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using QuizCraft.Domain.API.Controllers;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Tests.Unit.ControllersTests;

public class StatisticsControllerTest
{
    private readonly Mock<IStatisticsService> _statisticsServiceMock;
    private readonly StatisticsController _controller;

    public StatisticsControllerTest()
    {
        _statisticsServiceMock = new Mock<IStatisticsService>();
        _controller = new StatisticsController(_statisticsServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public async Task GetQuizAttemptsForUser_ReturnsExpectedResult()
    {
        // Arrange
        var token = "test-token";
        var userId = Guid.NewGuid();
        var quizAttemptsDto = new QuizAttemptsDto
        {
            Name = "Test Quiz",
            Answers = 1,
            Attempts =
                [
                    new QuizAttemptDto
                    {
                        Id = Guid.NewGuid(),
                        StartedAt = DateTime.Now,
                        CorrectAnswers = 1
                    }
                ]
        };

        _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";
        _statisticsServiceMock.Setup(s => s.QuizAttemptsForUser(token, userId)).ReturnsAsync(quizAttemptsDto);

        // Act
        var result = await _controller.GetQuizAttemptsForUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<QuizAttemptsDto>(okResult.Value);
        Assert.Equal("Test Quiz", returnValue.Name);
        Assert.Equal(1, returnValue.Answers);
        Assert.Single(returnValue.Attempts);
        Assert.Equal(1, returnValue.Attempts[0].CorrectAnswers);
    }
}