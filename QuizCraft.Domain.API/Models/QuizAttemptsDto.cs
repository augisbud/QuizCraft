using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Models;

public class QuizAttemptsDto
{
    [Required]
    public required string Name { get; set; }
    
    [Required]
    public required int Answers { get; set; }

    [Required]
    public required List<QuizAttemptDto> Attempts { get; set; } = [];
}