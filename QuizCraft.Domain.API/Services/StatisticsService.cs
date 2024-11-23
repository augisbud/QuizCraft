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
}