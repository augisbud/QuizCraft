using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Models;

public class ValidatedAnswerDto
{
    [Required] 
    public required AnswerAttemptDto SelectedAnswer { get; set; }
    
    [Required] 
    public required AnswerDto CorrectAnswer { get; set; }
}