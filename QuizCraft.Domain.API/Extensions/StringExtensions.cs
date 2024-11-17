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
}