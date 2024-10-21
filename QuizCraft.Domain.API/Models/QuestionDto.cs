using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Models;

public class QuestionDto
{
    [Required]
    public required Guid Id { get; set; }

    [Required]
    public required string Text { get; set; }

    [Required]
    public required List<string> Answers { get; set; }
}