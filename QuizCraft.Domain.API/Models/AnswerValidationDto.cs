using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Models;

// 2. Creating and using your own class
public class AnswerValidationDto
{
    [Required]
    public required string Selected { get; set; }

    [Required]
    public required bool IsCorrect { get; set; }

    [Required]
    public required AnswerDto CorrectAnswer { get; set; }
}