using System.Net;
using Newtonsoft.Json;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Tests.Integration.Fixtures;

namespace QuizCraft.Domain.API.Tests.Integration;

public class ControllerTests(ControllerTestsFixture fixture) : IClassFixture<ControllerTestsFixture>
{
    [Fact]
    public async Task GetQuizes_ReturnsExpected()
    {
        // Arrange
        var client = fixture.Factory.CreateClient();

        // Act
        var response = await client.GetAsync("/quizes?topic=cities");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var data = JsonConvert.DeserializeObject<QuestionDto>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(data);
        Assert.Equal("What is the capital of France?", data.Text);
        Assert.Equal(2, data.Answers.Count);
        Assert.Equal("Paris", data.Answers[0].Text);
        Assert.Equal("Madrid", data.Answers[1].Text);
    }
}