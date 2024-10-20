namespace QuizCraft.Domain.API.Models;

public class QuestionForCreationDto
{
    public required string Text { get; set; }
    public required List<AnswerDto> Answers { get; set; }
}