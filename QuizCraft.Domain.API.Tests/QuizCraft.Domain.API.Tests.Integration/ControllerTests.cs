using System.Net;
using Newtonsoft.Json;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Tests.Integration.Fixtures;

namespace QuizCraft.Domain.API.Tests.Integration;

public class ControllerTests(ControllerTestsFixture fixture) : IClassFixture<ControllerTestsFixture>
{
    private readonly ControllerTestsFixture _fixture;

    public ControllerTests(ControllerTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetQuizes_ReturnsExpected()
    {
        // Arrange
        var client = fixture.Factory.CreateClient();
        var fileContent = "The capital of France is Paris.";

        // Act
        var response = await client.GetAsync($"/quizzes?fileContent={WebUtility.UrlEncode(fileContent)}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var data = JsonConvert.DeserializeObject<QuestionDto>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(data);
        Assert.Equal("What is the capital of France?", data.Text);
        Assert.Equal(4, data.Answers.Count);

        Assert.Contains(data.Answers, x => x.Text == "Paris");
        Assert.Contains(data.Answers, x => x.Text == "London");
        Assert.Contains(data.Answers, x => x.Text == "Berlin");
        Assert.Contains(data.Answers, x => x.Text == "Madrid");
    }
}