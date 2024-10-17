namespace QuizCraft.Domain.API.Models;

// 2. Creating and using your own class
// 10. Implement at least one of the standard .NET interfaces (IEnumerable, IComparable, IComparer, IEquatable, IEnumerator, etc.)
public class QuizDto : IComparable<QuizDto>
{
    public required Guid Id { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required List<QuestionDto> Questions { get; set; }

    public int CompareTo(QuizDto? other)
    {
        if (other == null) return 1;

        int questionCountComparison = this.Questions.Count.CompareTo(other.Questions.Count);
        if (questionCountComparison != 0) return questionCountComparison;

        for (int i = 0; i < this.Questions.Count; i++)
        {
            int questionComparison = this.Questions[i].CompareTo(other.Questions[i]);
            if (questionComparison != 0) return questionComparison;
        }

        return 0;
    }
}