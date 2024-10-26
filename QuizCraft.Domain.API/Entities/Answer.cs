using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Entities;

public class Answer
{
    [Key, Required]
    public Guid Id { get; set; }

    [Required]
    public string Text { get; set; } = "";

    [Required]
    public bool IsCorrect { get; set; }

    [Required]
    public Guid QuestionId { get; set; }
    

    public Question Question { get; set; } = null!;
}