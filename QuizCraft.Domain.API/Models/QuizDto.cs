using QuizCraft.Domain.API.Constants;

namespace QuizCraft.Domain.API.Models;

public class QuizDto
{
    public required Guid Id { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required string Title { get; set; }

    public required Category Category { get; set; }

    public int QuestionCount { get; set; }
}