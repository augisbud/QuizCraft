using System.Net;
using System.Text;
using Newtonsoft.Json;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Tests.Integration.Fixtures;

namespace QuizCraft.Domain.API.Tests.Integration;

public class ControllerTests(ControllerTestsFixture fixture) : IClassFixture<ControllerTestsFixture>
{
    [Fact]
    public async Task CreateQuiz_ReturnsExpected()
    {
        // Arrange
        var client = fixture.Factory.CreateClient();

        // Act
        var response = await client.PostAsync("/quiz", new StringContent(JsonConvert.SerializeObject("A very extensive source about cities"), Encoding.UTF8, "application/json"));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var data = JsonConvert.DeserializeObject<QuizDto>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(data);
        Assert.Single(data.Questions);
        Assert.Equal("What is the capital of France?", data.Questions[0].Text);
        Assert.Equal(2, data.Questions[0].Answers.Count);
        Assert.Equal("Paris", data.Questions[0].Answers[0].Text);
        Assert.Equal("Madrid", data.Questions[0].Answers[1].Text);
    }

    [Fact]
    public async Task RetrieveQuizzes_ReturnsExpected()
    {
        // Arrange
        var client = fixture.Factory.CreateClient();

        // Act
        var response = await client.GetAsync("/quizzes");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var data = JsonConvert.DeserializeObject<List<QuizDto>>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(data);
        Assert.Single(data);
        Assert.Equal(DataFixture.Quizzes.First().Id, data[0].Id);
        Assert.Single(data[0].Questions);
        Assert.Equal("What is 2 + 2?", data[0].Questions[0].Text);
        Assert.Equal(2, data[0].Questions[0].Answers.Count);
        Assert.Equal("4", data[0].Questions[0].Answers[0].Text);
        Assert.Equal("5", data[0].Questions[0].Answers[1].Text);
    }
}