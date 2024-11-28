using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QuizCraft.Domain.API.Data;
using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Models;

namespace QuizCraft.Domain.API.Repositories;

public class QuizRepository(QuizzesDbContext context, IMapper mapper) : BaseRepository<Quiz>(context, mapper), IQuizRepository
{
    private readonly IMapper _mapper = mapper;

    public Quiz? RetrieveQuizWithQuestionsById(Guid id)
    {
        return _dbSet
            .AsSplitQuery()
            .Include(quiz => quiz.Questions)
            .ThenInclude(question => question.Answers)
            .Include(quiz => quiz.QuizAttempts)
            .ThenInclude(attempt => attempt.QuizAnswerAttempts)
            .FirstOrDefault(quiz => quiz.Id == id);
    }

    public IEnumerable<QuestionDto> RetrieveQuestions(Guid quizId)
    {
        return _context.Set<Question>()
            .Where(q => q.QuizId == quizId)
            .ProjectTo<QuestionDto>(_mapper.ConfigurationProvider);
    }

    public AnswerDto? RetrieveAnswer(Guid quizId, Guid questionId)
    {
        return _context.Set<Answer>()
            .Where(answer => answer.Question.QuizId == quizId && answer.QuestionId == questionId && answer.IsCorrect)
            .ProjectTo<AnswerDto>(_mapper.ConfigurationProvider)
            .FirstOrDefault();
    }

    public async Task<int> GetTotalUsersAsync()
    {
        return await context.QuizAttempts
            .Select(q => q.UserEmail)
            .Distinct()
            .CountAsync();
    }

    public async Task<int> GetTotalQuizzesCreatedAsync()
    {
        return await context.Quizzes.CountAsync();
    }

    public async Task<double> GetAverageQuizzesTakenPerUserAsync()
    {
        var totalUsers = await GetTotalUsersAsync();
        var totalAttempts = await context.QuizAttempts.CountAsync();

        return totalUsers == 0 ? 0 : (double)totalAttempts / totalUsers;
    }
}