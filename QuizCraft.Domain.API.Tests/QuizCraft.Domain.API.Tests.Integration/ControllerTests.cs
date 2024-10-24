using System.Net;
using System.Text;
using Newtonsoft.Json;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Tests.Integration.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using QuizCraft.Domain.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace QuizCraft.Domain.API.Tests.Integration;

public class ControllerTests(ControllerTestsFixture fixture) : IClassFixture<ControllerTestsFixture>
{
    [Fact]
    public async Task CreateQuiz_ReturnsExpected()
    {
        // Arrange
        var client = fixture.Factory.CreateClient();

        // Create a test file in memory
        var fileContent = "The capital of France is Paris.";
        using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
        var file = new FormFile(contentStream, 0, contentStream.Length, "file", "test.txt");

        // Create a multipart form data content
        var formData = new MultipartFormDataContent
        {
            { new StreamContent(contentStream), "file", "test.txt" }
        };

        // Act
        var response = await client.PostAsync("/quizzes", formData);

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

        var requestContent = new StringContent("{}", Encoding.UTF8, "application/json");
        // Act
        var response = await client.GetAsync("/quizzes");

        // Log the quizzes
        using var scope = fixture.Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<QuizzesDbContext>();
        var allQuizzes = await context.Quizzes.ToListAsync();
        Console.WriteLine(JsonConvert.SerializeObject(allQuizzes));

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