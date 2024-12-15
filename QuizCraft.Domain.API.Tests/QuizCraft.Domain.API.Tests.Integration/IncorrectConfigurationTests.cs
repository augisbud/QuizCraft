using System.Net;
using Newtonsoft.Json;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Tests.Integration.Fixtures;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace QuizCraft.Domain.API.Tests.Integration;

public class IncorrectConfigurationTests : IClassFixture<IncorrectConfigurationTestsFixture>
{
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();
    private readonly IncorrectConfigurationTestsFixture _fixture;

    public IncorrectConfigurationTests(IncorrectConfigurationTestsFixture fixture)
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
    public async Task GetQuizAttemptsForUser_ReturnsInternalServerError()
    {
        // Arrange
        var client = _fixture.Factory.CreateClient();
        var token = GenerateJwtToken(_jwtSecurityTokenHandler, "student1@example.com");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var id = DataFixture.Quiz1Id;

        // Act
        var response = await client.GetAsync($"/statistics/individual/quizzes/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

        var data = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());

        Assert.NotNull(data);
        Assert.Equal(StatusCodes.Status500InternalServerError, data.Status);
        Assert.Equal("Internal Server Error", data.Title);
    }
}