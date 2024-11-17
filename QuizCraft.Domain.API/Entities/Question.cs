using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Entities;

public class Question
{
    [Key, Required]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public required string Text { get; set; }

    [Required]
    public required Guid QuizId { get; set; }


    public Quiz Quiz { get; set; } = null!;
    public ICollection<Answer> Answers { get; set; } = [];
}