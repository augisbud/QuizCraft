using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Models;

namespace QuizCraft.Domain.API.Repositories;

public interface IQuizRepository
{
    Task<Quiz> CreateQuizAsync(Quiz quiz);
    QuizDto? RetrieveQuizById(Guid id);
    IEnumerable<QuestionDto> RetrieveQuestions(Guid quizId, string userEmail);
    AnswerDto? RetrieveAnswer(Guid quizId, Guid questionId);
    IEnumerable<QuizDto> RetrieveQuizzes();
    Task<QuizAnswerAttempt> CreateQuizAnswerAttemptAsync(QuizAnswerAttempt attempt);
}