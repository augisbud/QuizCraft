using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using QuizCraft.Domain.API.Exceptions;
using QuizCraft.Domain.API.Extensions;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Repositories;

namespace QuizCraft.Domain.API.Services;

public class StatisticsService(IQuizRepository quizRepository, IQuizAttemptRepository quizAttemptRepository, JwtSecurityTokenHandler jwtSecurityTokenHandler, IMapper mapper) : IStatisticsService
{
    public async Task<QuizAttemptsDto> QuizAttemptsForUser(string token, Guid quizId)
    {
        var quiz = await quizRepository.RetrieveByIdAsync(quizId, e => e.Questions) ?? throw new QuizNotFoundException(quizId);
        var data = quizAttemptRepository.RetrieveQuizAttempts(quizId, token.RetrieveEmail(jwtSecurityTokenHandler));

        return new()
        {
            Name = quiz.Title,
            Answers = quiz.Questions.Count,
            Attempts = mapper.Map<List<QuizAttemptDto>>(data)
        };
    }

    public async Task<GlobalStatsDto> GlobalStatisticsAsync()
    {
        var userEmailsQuery = (await quizAttemptRepository.RetrieveAllAsync())
            .Select(attempt => attempt.UserEmail)
            .Distinct();

        var totalQuizzesQuery = await quizRepository.RetrieveAllAsync();
        var totalAttemptsQuery = await quizAttemptRepository.RetrieveAllAsync();

        var totalUsers = userEmailsQuery.Count();
        var totalQuizzes = totalQuizzesQuery.Count();
        var totalAttempts = totalAttemptsQuery.Count();

        var averageQuizzesPerUser = totalUsers == 0 ? 0 : (double)totalAttempts / totalUsers;

        return new GlobalStatsDto
        {
            TotalUsers = totalUsers,
            TotalQuizzesCreated = totalQuizzes,
            AverageQuizzesPerUser = averageQuizzesPerUser
        };
    }
}