using QuizCraft.Domain.API.Models;

namespace QuizCraft.Domain.API.Services;

public interface IStatisticsService
{
    Task<QuizAttemptsDto> QuizAttemptsForUser(string token, Guid quizId);
    Task<IEnumerable<StatisticDto>> GetGlobalStatisticsAsync();
}