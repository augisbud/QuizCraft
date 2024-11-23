using System.Net;
using System.Text;
using Newtonsoft.Json;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Tests.Integration.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using QuizCraft.Domain.API.Data;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace QuizCraft.Domain.API.Tests.Integration;

public class QuizControllerTests : IClassFixture<ControllerTestsFixture>
{
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();
    private readonly ControllerTestsFixture _fixture;

    public QuizControllerTests(ControllerTestsFixture fixture)
    {
        _fixture = fixture;
    }

    private static string GenerateJwtToken(JwtSecurityTokenHandler jwtSecurityTokenHandler, string email)
    {
        var token = jwtSecurityTokenHandler.WriteToken(new JwtSecurityToken(claims:
        [
            new Claim("email", email)
        ]));

        return token;
    }

    [Fact]
    public async Task GetQuizzes_ReturnsExpected()
    {
        // Arrange
        var client = _fixture.Factory.CreateClient();
        var token = GenerateJwtToken(_jwtSecurityTokenHandler, "user@example.com");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await client.GetAsync("/quizzes");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var data = JsonConvert.DeserializeObject<List<QuizDto>>(await response.Content.ReadAsStringAsync());

        Assert.NotNull(data);
        Assert.Single(data); // Would be 2, but we already delete one quiz.

        foreach (var quiz in data)
        {
            var matchedQuiz = DataFixture.Quizzes.FirstOrDefault(q => q.Id == quiz.Id);
            Assert.NotNull(matchedQuiz);
            Assert.Equal(quiz.Title, matchedQuiz.Title);
            Assert.Equal(quiz.Category, matchedQuiz.Category);
        }
    }

    [Fact]
    public async Task GetDetailedQuizDto_ReturnsExpected()
    {
        // Arrange
        var client = _fixture.Factory.CreateClient();
        var token = GenerateJwtToken(_jwtSecurityTokenHandler, "user@example.com");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var quizId = DataFixture.Quiz1Id;

        // Act
        var response = await client.GetAsync($"/quizzes/{quizId}/questions");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var data = JsonConvert.DeserializeObject<DetailedQuizDto>(await response.Content.ReadAsStringAsync());

        Assert.NotNull(data);
        Assert.Equal(quizId, data.Id);
        var expectedQuiz = DataFixture.Quizzes.First(q => q.Id == quizId);
        Assert.Equal(expectedQuiz.Title, data.Title);
    }

    [Fact]
    public async Task ValidateAnswer_ReturnsExpected()
    {
        // Arrange
        var client = _fixture.Factory.CreateClient();
        var token = GenerateJwtToken(_jwtSecurityTokenHandler, "user@example.com");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var quizId = DataFixture.Quiz1Id;
        var questionId = DataFixture.Question1Id;
        var inputDto = new AnswerAttemptDto(DataFixture.Answer1Id);
        var content = new StringContent(JsonConvert.SerializeObject(inputDto), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"/quizzes/{quizId}/questions/{questionId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var data = JsonConvert.DeserializeObject<ValidatedAnswerDto>(await response.Content.ReadAsStringAsync());

        Assert.NotNull(data);
    }

    [Fact]
    public async Task CompleteQuizAttempt_ReturnsOk()
    {
        // Arrange
        var client = _fixture.Factory.CreateClient();
        var token = GenerateJwtToken(_jwtSecurityTokenHandler, "user@example.com");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var quizId = DataFixture.Quiz1Id;

        // Act
        var response = await client.PostAsync($"/quizzes/{quizId}", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var scope = _fixture.Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<QuizzesDbContext>();
        var quizAttempt = await context.QuizAttempts.FirstOrDefaultAsync(q => q.QuizId == quizId && q.IsCompleted);
        Assert.NotNull(quizAttempt);
        Assert.True(quizAttempt.IsCompleted);
    }

    [Fact]
    public async Task DeleteQuiz_ReturnsOk()
    {
        // Arrange
        var client = _fixture.Factory.CreateClient();
        var token = GenerateJwtToken(_jwtSecurityTokenHandler, "user@example.com");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var quizId = DataFixture.Quiz2Id;

        // Act
        var response = await client.DeleteAsync($"/quizzes/{quizId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var scope = _fixture.Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<QuizzesDbContext>();
        var deletedQuiz = await context.Quizzes.FindAsync(quizId);
        Assert.Null(deletedQuiz);
    }

    [Fact]
    public async Task GetQuizAttemptsForUser_ReturnsOk()
    {
        // Arrange
        var client = _fixture.Factory.CreateClient();
        var token = GenerateJwtToken(_jwtSecurityTokenHandler, "student1@example.com");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var id = DataFixture.Quiz1Id;

        // Act
        var response = await client.GetAsync($"/statistics/individual/quizzes/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var data = JsonConvert.DeserializeObject<QuizAttemptsDto>(await response.Content.ReadAsStringAsync());

        Assert.NotNull(data);
        Assert.Equal(DataFixture.Quizzes.Where(x => x.Id == id).First().Title, data.Name);
        Assert.Equal(DataFixture.Quizzes.Where(x => x.Id == id).First().Questions.Count, data.Answers);
        Assert.Single(data.Attempts);
        Assert.Equal(DataFixture.QuizAttempts.Where(x => x.QuizId == id).First().Id, data.Attempts.First().Id);
        Assert.Equal(2, data.Attempts.First().CorrectAnswers);
    }
}