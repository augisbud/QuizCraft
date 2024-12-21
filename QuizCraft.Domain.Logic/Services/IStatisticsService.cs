using QuizCraft.Domain.Data.Models;

namespace QuizCraft.Domain.Logic.Services;

public interface IStatisticsService
{
    Task<QuizAttemptsDto> QuizAttemptsForUser(string token, Guid quizId);
    Task<GlobalStatsDto> GlobalStatisticsAsync();
}