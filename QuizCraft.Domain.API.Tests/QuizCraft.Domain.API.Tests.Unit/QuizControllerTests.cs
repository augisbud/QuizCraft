using Microsoft.AspNetCore.Mvc;
using QuizCraft.Domain.API.Controllers;

namespace QuizCraft.Domain.API.Tests.Unit;

public class QuizControllerTests
{
    private readonly QuizController _controller = new();

    [Fact]
    public void GetQuizes_ReturnsExpected()
    {
        // Act
        var response = _controller.GetQuizes();

        // Assert
        var result = Assert.IsType<OkObjectResult>(response);
        var data = Assert.IsAssignableFrom<List<string>>(result.Value);
        Assert.Equal(2, data.Count);
        Assert.Equal("Quiz 1", data[0]);
        Assert.Equal("Quiz 2", data[1]);
    }
}