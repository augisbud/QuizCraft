using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Entities;

public class QuizAnswerAttempt : IEntity
{
    [Key, Required]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public required Guid QuizAttemptId { get; set; }

    [Required]
    public required Guid QuestionId { get; set; }

    [Required]
    public required Guid AnswerId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public QuizAttempt QuizAttempt { get; set; } = null!;
    public Question Question { get; set; } = null!;
    public Answer Answer { get; set; } = null!;
}