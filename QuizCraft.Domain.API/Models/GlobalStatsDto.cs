namespace QuizCraft.Domain.API.Models;

public class GlobalStatsDto
{
    public int TotalUsers { get; set; }
    public int TotalQuizzesCreated { get; set; }
    public double AverageQuizzesPerUser { get; set; }
}