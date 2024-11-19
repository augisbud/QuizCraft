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
}