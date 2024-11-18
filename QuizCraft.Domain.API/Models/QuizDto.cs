using System.ComponentModel.DataAnnotations;
using QuizCraft.Domain.API.Constants;

namespace QuizCraft.Domain.API.Models;

public class QuizDto
{
    [Required]
    public required Guid Id { get; set; }

    [Required]
    public required DateTime CreatedAt { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public required Category Category { get; set; }

    [Required]
    public int QuestionCount { get; set; }

    [Required]
    public int CompletedBy { get; set; }

    [Required]
    public int? AverageScore { get; set; }

    [Required]
    public int? HighScore { get; set; }

    [Required]
    public bool IsOwner { get; set; }
}