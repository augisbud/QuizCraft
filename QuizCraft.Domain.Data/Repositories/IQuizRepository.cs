using QuizCraft.Domain.Data.Models;
using QuizCraft.Domain.Data.Entities;

namespace QuizCraft.Domain.Data.Repositories;

public interface IQuizRepository : IBaseRepository<Quiz>
{
    Quiz? RetrieveQuizWithQuestionsById(Guid id);
    IEnumerable<QuestionDto> RetrieveQuestions(Guid quizId);
    AnswerDto? RetrieveAnswer(Guid quizId, Guid questionId);
    Task<int> GetTotalUsersAsync();
    Task<int> GetTotalQuizzesCreatedAsync();
    Task<double> GetAverageQuizzesTakenPerUserAsync();
}