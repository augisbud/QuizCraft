using QuizCraft.Domain.Data.Entities;

namespace QuizCraft.Domain.Data.Repositories;

public interface IQuizAnswerAttemptRepository : IBaseRepository<QuizAnswerAttempt>
{
    IEnumerable<QuizAnswerAttempt> RetrieveQuizAnswerAttempts(Guid attemptId);
    QuizAnswerAttempt CreateQuizAnswerAttempt(Guid attemptId, Guid questionId, Guid answerId);
}