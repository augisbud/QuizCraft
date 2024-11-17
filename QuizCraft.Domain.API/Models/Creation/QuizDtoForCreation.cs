using System.ComponentModel.DataAnnotations;
using QuizCraft.Domain.API.Constants;

namespace QuizCraft.Domain.API.Models;

public class QuizDtoForCreation
{
    [Required]
    public required string Title { get; set; }

    [Required]
    public required Category Category { get; set; }

    [Required]
    public required IEnumerable<QuestionForCreationDto> Questions { get; set; } = [];
}