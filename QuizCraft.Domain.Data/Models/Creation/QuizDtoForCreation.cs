using System.ComponentModel.DataAnnotations;
using QuizCraft.Domain.Data.Constants;

namespace QuizCraft.Domain.Data.Models;

public class QuizDtoForCreation
{
    [Required]
    public required string Title { get; set; }

    [Required]
    public required Category Category { get; set; }

    [Required]
    public required IEnumerable<QuestionForCreationDto> Questions { get; set; } = [];
}