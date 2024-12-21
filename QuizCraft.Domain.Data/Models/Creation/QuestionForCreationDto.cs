using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.Data.Models;

public class QuestionForCreationDto
{
    [Required]
    public required string Text { get; set; }

    [Required]
    public required List<AnswerForCreationDto> Answers { get; set; }= [];
}