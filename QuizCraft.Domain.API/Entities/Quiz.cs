using System.ComponentModel.DataAnnotations;
using QuizCraft.Domain.API.Constants;

namespace QuizCraft.Domain.API.Entities;

public class Quiz
{
    [Key, Required]
    public Guid Id { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public required Category Category { get; set; }

    [Required]
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    public IEnumerable<Question> Questions { get; set; } = [];
}