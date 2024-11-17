namespace QuizCraft.Domain.API.Models;

public class AnswerDto
{
    public required Guid Id { get; set; }
    public required string Text { get; set; }
}