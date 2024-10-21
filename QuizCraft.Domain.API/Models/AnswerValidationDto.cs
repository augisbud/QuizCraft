using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Models;

public class AnswerValidationDto
{
    [Required]
    public required string Selected { get; set; }

    [Required]
    public required bool IsCorrect { get; set; }

    [Required]
    public required AnswerDto CorrectAnswer { get; set; }
}