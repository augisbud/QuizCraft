using System.IdentityModel.Tokens.Jwt;

namespace QuizCraft.Domain.API.Extensions;

public static class StringExtensions
{
    public static string CleanJsonString(this string input)
    {
        return input.Replace("```json", "")
                    .Replace("```", "")
                    .Replace("\n", "")
                    .Trim();
    }

    public static string RetrieveEmail(this string token, JwtSecurityTokenHandler jwtSecurityTokenHandler)
    {
        var jwtToken = jwtSecurityTokenHandler.ReadJwtToken(token);

        return jwtToken.Claims.First(c => c.Type == "email").Value;
    }
}