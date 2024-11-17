using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Models;

public class QuizAttemptDto
{
    [Required]
    public required Guid Id { get; set; }

    [Required]
    public required DateTime StartedAt { get; set; }

    [Required]
    public int CorrectAnswers { get; set; }
}