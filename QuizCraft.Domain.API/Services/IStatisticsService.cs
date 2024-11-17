using QuizCraft.Domain.API.Models;

namespace QuizCraft.Domain.API.Services;

public interface IStatisticsService
{
    QuizAttemptsDto QuizAttemptsForUser(string token, Guid quizId);
}