using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Models;

public class QuizForTransferDto : QuizDto
{
    [Required]
    public required string CreatedBy { get; set; }
}