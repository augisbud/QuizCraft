using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using QuizCraft.Domain.API.Extensions;

namespace QuizCraft.Domain.API.Tests.Unit.ExtensionsTests;

public class StringExtensionsTest
{
    [Theory]
    [InlineData("```json{\"key\":\"value\"}```", "{\"key\":\"value\"}")]
    [InlineData("```json\n{\"key\":\"value\"}\n```", "{\"key\":\"value\"}")]
    [InlineData("{\"key\":\"value\"}", "{\"key\":\"value\"}")]
    public void CleanJsonString_Returns_CorrectString(string input, string expected)
    {
        // Act
        var result = input.CleanJsonString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void RetrieveEmail_Returns_CorrectEmail()
    {
        // Arrange
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(new JwtSecurityToken(claims: new[] { new Claim("email", "test@test.com") }));

        // Act
        var email = token.RetrieveEmail(tokenHandler);

        // Assert
        Assert.Equal("test@test.com", email);
    }
}