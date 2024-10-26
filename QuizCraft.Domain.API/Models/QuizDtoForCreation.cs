using System.ComponentModel.DataAnnotations;
using QuizCraft.Domain.API.Constants;

namespace QuizCraft.Domain.API.Models;

// 10. Implement at least one of the standard .NET interfaces (IComparable)
public class QuizDtoForCreation : IComparable<QuizDtoForCreation>
{
    [Required]
    public required string Title { get; set; }

    [Required]
    public required Category Category { get; set; }

    [Required]
    public required IEnumerable<QuestionForCreationDto> Questions { get; set; } = [];

    public int CompareTo(QuizDtoForCreation? other)
    {
        if (other == null) return 1;

        int titleComparison = string.Compare(this.Title, other.Title, StringComparison.Ordinal);
        if (titleComparison != 0) return titleComparison;

        int categoryComparison = this.Category.CompareTo(other.Category);
        if (categoryComparison != 0) return categoryComparison;

        int questionsCountComparison = this.Questions.Count().CompareTo(other.Questions.Count());
        if (questionsCountComparison != 0) return questionsCountComparison;

        using var thisEnumerator = this.Questions.GetEnumerator();
        using var otherEnumerator = other.Questions.GetEnumerator();
        while (thisEnumerator.MoveNext() && otherEnumerator.MoveNext())
        {
            int questionComparison = thisEnumerator.Current.CompareTo(otherEnumerator.Current);
            if (questionComparison != 0) return questionComparison;
        }

        return 0;
    }
}