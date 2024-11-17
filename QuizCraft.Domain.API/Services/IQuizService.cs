using QuizCraft.Domain.API.Models;

namespace QuizCraft.Domain.API.Services;

public interface IQuizService
{
    Task<QuizDto> CreateQuizAsync(string source);
    QuizDto RetrieveQuizById(Guid id);
    DetailedQuizDto RetrieveQuestions(Guid quizId, string token);
    IEnumerable<QuizDto> RetrieveQuizzes();
    ValidatedAnswerDto ValidateAnswer(string token, Guid quizId, Guid questionId, AnswerAttemptDto answerAttemptDto);
    void CompleteQuizAttempt(string token, Guid quizId);
}