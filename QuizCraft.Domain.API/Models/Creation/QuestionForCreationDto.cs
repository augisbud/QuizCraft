using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Models;

public class QuestionForCreationDto
{
    [Required]
    public required string Text { get; set; }

    [Required]
    public required List<AnswerForCreationDto> Answers { get; set; }= [];
}