using System.ComponentModel.DataAnnotations;

namespace QuizCraft.Domain.API.Entities;

public class Question
{
    [Key, Required]
    public Guid Id { get; set; }

    [Required]
    public string Text { get; set; } = "";

    public List<Answer> Answers { get; set; } = [];
}