using QuizCraft.Domain.API.Constants;

namespace QuizCraft.Domain.API.Models;

public class QuizDtoForCreation
{
    public required string Title { get; set; }

    public required Category Category { get; set; }

    public required IEnumerable<QuestionForCreationDto> Questions { get; set; } = [];
}