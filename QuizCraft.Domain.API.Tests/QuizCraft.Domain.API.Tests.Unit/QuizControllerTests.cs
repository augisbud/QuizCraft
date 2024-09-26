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
        var expectedOutput = new QuestionDto()
        {
            Text = "What is the capital of France?",
            Answers = [
                new()
                {
                    Text = "Paris"
                },
            ]
        };

        _quizService
            .Setup(x => x.GenerateQuiz("cities"))
            .ReturnsAsync(expectedOutput);

        // Act
        var response = await _controller.GetQuizes("cities");

        // Assert
        var result = Assert.IsType<OkObjectResult>(response.Result);
        var data = Assert.IsAssignableFrom<QuestionDto>(result.Value);
        Assert.Equal(expectedOutput, data);
    }
}