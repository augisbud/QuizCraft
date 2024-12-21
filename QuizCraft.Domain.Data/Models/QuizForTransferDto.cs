using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.Data.Models;

public class QuizForTransferDto : QuizDto
{
    [Required]
    public required string CreatedBy { get; set; }
}