using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Entities;

namespace QuizCraft.Domain.API.Repositories;

public interface IQuizRepository : IBaseRepository<Quiz>
{
    Quiz? RetrieveQuizWithQuestionsById(Guid id);
    IEnumerable<QuestionDto> RetrieveQuestions(Guid quizId);
    AnswerDto? RetrieveAnswer(Guid quizId, Guid questionId);
    Task<int> GetTotalUsersAsync();
    Task<int> GetTotalQuizzesCreatedAsync();
    Task<double> GetAverageQuizzesTakenPerUserAsync();
}