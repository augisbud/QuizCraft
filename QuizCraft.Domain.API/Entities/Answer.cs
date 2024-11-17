using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Entities;

public class Answer
{
    [Key, Required]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public required string Text { get; set; }

    [Required]
    public required bool IsCorrect { get; set; }

    [Required]
    public required Guid QuestionId { get; set; }
        

    public Question Question { get; set; } = null!;
}