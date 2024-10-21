using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Entities;

public class Question
{
    [Key, Required]
    public Guid Id { get; set; }

    [Required]
    public string Text { get; set; } = "";

    [Required]
    public Guid QuizId { get; set; }

    public Quiz Quiz { get; set; } = null!;
    public IEnumerable<Answer> Answers { get; set; } = [];
}