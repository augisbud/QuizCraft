using QuizCraft.Domain.Data.Entities;

namespace QuizCraft.Domain.Data.Repositories;

public interface IQuizAttemptRepository : IBaseRepository<QuizAttempt>
{
    QuizAttempt? RetrieveQuizAttempt(Guid quizId, string email);
    IEnumerable<QuizAttempt> RetrieveQuizAttempts(Guid quizId, string email);
    QuizAttempt CreateQuizAttempt(Guid quizId, string email);
}