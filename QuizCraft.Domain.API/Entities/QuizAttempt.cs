using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Entities;

public class QuizAttempt
{
    [Key, Required]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public required Guid QuizId { get; set; }

    [Required]
    public required string UserEmail { get; set; }

    [Required]
    public required bool IsCompleted { get; set; }

    [Required]
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    public Quiz Quiz { get; set; } = null!;
    public ICollection<QuizAnswerAttempt> QuizAnswerAttempts { get; set; } = [];
}