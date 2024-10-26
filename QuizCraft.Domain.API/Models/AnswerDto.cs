namespace QuizCraft.Domain.API.Models;

// 2. Creating and using your own record
public record AnswerDto(string Text, bool IsCorrect) : IComparable<AnswerDto>
{
    public int CompareTo(AnswerDto? other)
    {
        if (other == null) return 1;

        int textComparison = string.Compare(this.Text, other.Text, StringComparison.Ordinal);
        if (textComparison != 0) return textComparison;

        return this.IsCorrect.CompareTo(other.IsCorrect);
    }
}