namespace QuizCraft.Domain.API.Extensions;

// 5. Extension method usage
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