namespace QuizCraft.Domain.API.Models;

public class AnswerValidationDto
{
    public required string Selected { get; set; }
    public required bool IsCorrect { get; set; }
    public required AnswerDto CorrectAnswer { get; set; }
}