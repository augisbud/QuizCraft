using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using QuizCraft.Domain.API.Exceptions;
using QuizCraft.Domain.API.Extensions;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Repositories;

namespace QuizCraft.Domain.API.Services;

public class StatisticsService(IQuizRepository repository, JwtSecurityTokenHandler jwtSecurityTokenHandler, IMapper mapper) : IStatisticsService
{
    public QuizAttemptsDto QuizAttemptsForUser(string token, Guid quizId)
    {
        var quiz = repository.RetrieveQuizById(quizId) ?? throw new QuizNotFoundException(quizId);
        var data = repository.RetrieveQuizAttempts(quizId, token.RetrieveEmail(jwtSecurityTokenHandler));

        return new()
        {
            Name = quiz.Title,
            Answers = quiz.Questions.SelectMany(x => x.Answers).Count(),
            Attempts = mapper.Map<List<QuizAttemptDto>>(data)
        };
    }

    private static string RetrieveEmail(JwtSecurityTokenHandler jwtSecurityTokenHandler, string token)
    {
        var jwtToken = jwtSecurityTokenHandler.ReadJwtToken(token);

        return jwtToken.Claims.First(c => c.Type == "email").Value;
    }

    public async Task<GlobalStatsDto> GetGlobalStatisticsAsync()
    {
        var totalUsers = await repository.GetTotalUsersAsync();
        var totalQuizzesCreated = await repository.GetTotalQuizzesCreatedAsync();
        var quizzesPerUser = await repository.GetAverageQuizzesTakenPerUserAsync();

        return new GlobalStatsDto
        {
            TotalUsers = totalUsers,
            TotalQuizzesCreated = totalQuizzesCreated,
            AverageQuizzesPerUser = quizzesPerUser
        };
    }
}