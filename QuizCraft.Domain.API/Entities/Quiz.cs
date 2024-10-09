using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Entities;

public class Quiz
{
    [Key, Required]
    public Guid Id { get; set; }

    [Required]
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    public List<Question> Questions { get; set; } = [];
}