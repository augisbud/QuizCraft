using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Models;

public class QuestionForCreationDto : IComparable<QuestionForCreationDto>
{
    [Required]
    public required string Text { get; set; }

    // 3. Property usage in class
    [Required]
    public required List<AnswerDto> Answers 
    { 
        get { return _answers; } 
        set 
        {
            if (value == null || value.Count < 2)
            {
                throw new ArgumentException("A question must have at least 2 answers");
            }
            _answers = value;
        }
    }

    private List<AnswerDto> _answers = [];

    public int CompareTo(QuestionForCreationDto? other)
    {
        if (other == null) return 1;

        int textComparison = string.Compare(this.Text, other.Text, StringComparison.Ordinal);
        if (textComparison != 0) return textComparison;

        int answersCountComparison = this.Answers.Count.CompareTo(other.Answers.Count);
        if (answersCountComparison != 0) return answersCountComparison;

        for (int i = 0; i < this.Answers.Count; i++)
        {
            int answerComparison = this.Answers[i].CompareTo(other.Answers[i]);
            if (answerComparison != 0) return answerComparison;
        }

        return 0;
    }
}