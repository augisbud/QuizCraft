using QuizCraft.Domain.API.Entities;

namespace QuizCraft.Domain.API.Repositories;

public interface IQuizAnswerAttemptRepository : IBaseRepository<QuizAnswerAttempt>
{
    IEnumerable<QuizAnswerAttempt> RetrieveQuizAnswerAttempts(Guid attemptId);
    QuizAnswerAttempt CreateQuizAnswerAttempt(Guid attemptId, Guid questionId, Guid answerId);
}