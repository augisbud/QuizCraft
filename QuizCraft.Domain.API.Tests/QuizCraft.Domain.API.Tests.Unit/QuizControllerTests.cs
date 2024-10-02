using Microsoft.AspNetCore.Mvc;
using Moq;
using QuizCraft.Domain.API.Controllers;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Tests.Unit;

public class QuizControllerTests
{
    private readonly Mock<IQuizService> _quizService = new();
    private readonly QuizController _controller;
    
    public QuizControllerTests()
    {
        _controller = new(_quizService.Object);
    }

    [Fact]
    public async void GetQuizes_ReturnsExpected()
    {
        // Arrange
        var expectedOutput = new QuizDto()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Questions = [
                new()
                {
                    Text = "What is the capital of France?",
                    Answers = [
                        new()
                        {
                            Text = "Paris"
                        },
                    ]
                }
            ]
        };

        _quizService
            .Setup(x => x.CreateQuiz("A very extensive source about cities"))
            .ReturnsAsync(expectedOutput);

        // Act
        var response = await _controller.CreateQuiz("A very extensive source about cities");

        // Assert
        var result = Assert.IsType<OkObjectResult>(response.Result);
        var data = Assert.IsAssignableFrom<QuizDto>(result.Value);
        Assert.Equal(expectedOutput, data);
    }
}