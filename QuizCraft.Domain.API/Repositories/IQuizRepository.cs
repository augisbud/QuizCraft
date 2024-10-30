using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Models;

namespace QuizCraft.Domain.API.Repositories;

public interface IQuizRepository
{
    Task<Quiz> CreateQuizAsync(Quiz quiz);
    QuizDto? RetrieveQuizById(Guid id);
    IEnumerable<Question> RetrieveQuestions(Guid quizId);
    AnswerDto? RetrieveAnswer(Guid quizId, Guid questionId);
    IEnumerable<QuizDto> RetrieveQuizzes();
    Task<QuizAnswerAttempt> CreateQuizAnswerAttemptAsync(QuizAnswerAttempt attempt);
    IEnumerable<QuizAnswerAttempt> RetrieveQuizAnswerAttempts(Guid quizId);
    IEnumerable<QuizAnswerAttempt> RetrieveAttemptsForQuestion(Guid quizId, Guid questionId);
}