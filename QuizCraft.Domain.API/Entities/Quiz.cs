using System.ComponentModel.DataAnnotations;
using QuizCraft.Domain.API.Constants;

namespace QuizCraft.Domain.API.Entities;

public class Quiz : IEntity
{
    [Key, Required]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public required string Title { get; set; }

    [Required]
    public required Category Category { get; set; }

    [Required]
    public required string CreatedBy { get; set; }

    [Required]
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    
    public ICollection<Question> Questions { get; set; } = [];
    public ICollection<QuizAttempt> QuizAttempts { get; set; } = [];
}