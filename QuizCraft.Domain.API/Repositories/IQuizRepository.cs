using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Models;

namespace QuizCraft.Domain.API.Repositories;

public interface IQuizRepository
{
    Task<Quiz> CreateQuizAsync(Quiz quiz);
    QuizDto? RetrieveQuizById(Guid id);
    Quiz? RetrieveQuizWithQuestionsById(Guid id);
    IEnumerable<QuestionDto> RetrieveQuestions(Guid quizId);
    AnswerDto? RetrieveAnswer(Guid quizId, Guid questionId);
    IEnumerable<QuizDto> RetrieveQuizzes();
    QuizAttempt? RetrieveQuizAttempt(Guid quizId, string email);
    QuizAttempt CreateQuizAttempt(Guid quizId, string email);
    IEnumerable<QuizAnswerAttempt> RetrieveQuizAnswerAttempt(Guid attemptId);
    QuizAnswerAttempt CreateQuizAnswerAttempt(Guid attemptId, Guid questionId, Guid answerId);
    bool SaveChanges();
}