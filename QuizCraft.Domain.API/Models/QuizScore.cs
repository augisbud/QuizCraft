namespace QuizCraft.Domain.API.Models;

public struct QuizScore(Guid quizId, int score)
{
    public Guid QuizId { get; set; } = quizId;
    public int Score { get; set; } = score;
}