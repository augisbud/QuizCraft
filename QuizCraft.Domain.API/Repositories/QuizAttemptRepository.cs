using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizCraft.Domain.API.Data;
using QuizCraft.Domain.API.Entities;

namespace QuizCraft.Domain.API.Repositories;

public class QuizAttemptRepository(QuizzesDbContext context, IMapper mapper) : BaseRepository<QuizAttempt>(context, mapper), IQuizAttemptRepository
{
    public QuizAttempt? RetrieveQuizAttempt(Guid quizId, string email)
    {
        return _dbSet
            .Include(attempt => attempt.QuizAnswerAttempts)
            .FirstOrDefault(attempt => attempt.QuizId == quizId && attempt.UserEmail == email && !attempt.IsCompleted);
    }

    public IEnumerable<QuizAttempt> RetrieveQuizAttempts(Guid quizId, string email)
    {
        return _dbSet
            .Include(attempt => attempt.QuizAnswerAttempts)
            .ThenInclude(answerAttempt => answerAttempt.Answer)
            .Where(attempt => attempt.QuizId == quizId && attempt.UserEmail == email)
            .OrderBy(attempt => attempt.StartedAt);
    }

    public QuizAttempt CreateQuizAttempt(Guid quizId, string email)
    {
        var result = _dbSet.Add(new QuizAttempt
        {
            QuizId = quizId,
            UserEmail = email,
            IsCompleted = false
        });

        _context.SaveChanges();

        return result.Entity;
    }
}