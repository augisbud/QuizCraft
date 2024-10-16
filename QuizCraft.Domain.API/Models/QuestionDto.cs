namespace QuizCraft.Domain.API.Models;

public class QuestionDto : IComparable<QuestionDto>
{
    public required string Text { get; set; }
    public required List<AnswerDto> Answers { get; set; }

    public int CompareTo(QuestionDto? other)
    {
        if (other == null) return 1;

        int questionComparison = this.Text.CompareTo(other.Text);
        if (questionComparison != 0) return questionComparison;

        int answerCountComparison = this.Answers.Count.CompareTo(other.Answers.Count);
        if (answerCountComparison != 0) return answerCountComparison;

        for (int i = 0; i < this.Answers.Count; i++)
        {
            int answerComparison = this.Answers[i].CompareTo(other.Answers[i]);
            if (answerComparison != 0) return answerComparison;
        }

        return 0;
    }
}