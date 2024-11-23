using AutoMapper;
using QuizCraft.Domain.API.Data;
using QuizCraft.Domain.API.Entities;

namespace QuizCraft.Domain.API.Repositories;

public class QuizAnswerAttemptRepository(QuizzesDbContext context, IMapper mapper) : BaseRepository<QuizAnswerAttempt>(context, mapper), IQuizAnswerAttemptRepository
{
    public IEnumerable<QuizAnswerAttempt> RetrieveQuizAnswerAttempts(Guid attemptId)
    {
        return _dbSet
            .Where(attempt => attempt.QuizAttemptId == attemptId);
    }

    public QuizAnswerAttempt CreateQuizAnswerAttempt(Guid attemptId, Guid questionId, Guid answerId)
    {
        var result = _dbSet.Add(new QuizAnswerAttempt
        {
            QuizAttemptId = attemptId,
            QuestionId = questionId,
            AnswerId = answerId
        });

        _context.SaveChanges();

        return result.Entity;
    }
}
