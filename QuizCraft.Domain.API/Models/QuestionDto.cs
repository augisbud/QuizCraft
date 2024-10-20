namespace QuizCraft.Domain.API.Models;

public class QuestionDto
{
    public required Guid Id { get; set; }
    public required string Text { get; set; }
    public required List<string> Answers { get; set; }
}