using QuizCraft.Domain.API.Entities;

namespace QuizCraft.Domain.API.Repositories;

public interface IQuizAttemptRepository : IBaseRepository<QuizAttempt>
{
    QuizAttempt? RetrieveQuizAttempt(Guid quizId, string email);
    IEnumerable<QuizAttempt> RetrieveQuizAttempts(Guid quizId, string email);
    QuizAttempt CreateQuizAttempt(Guid quizId, string email);
}