namespace QuizCraft.Domain.API.Models;

public class QuizDto
{
    public required Guid Id { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required List<QuestionDto> Questions { get; set; }
}