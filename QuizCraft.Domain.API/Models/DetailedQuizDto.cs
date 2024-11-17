using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Models;

public class DetailedQuizDto
{
    [Required]
    public required Guid Id { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public required Guid? CurrentQuestionId { get; set; }

    [Required]
    public required IEnumerable<QuestionDto> Questions { get; set; }
}