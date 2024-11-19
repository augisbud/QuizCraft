using QuizCraft.Domain.API.Models;

namespace QuizCraft.Domain.API.Services;

public interface IQuizService
{
    Task<Guid> CreateQuizAsync(string source, string token);
    IEnumerable<QuizDto> RetrieveQuizzes(string? token);
    DetailedQuizDto RetrieveQuestions(Guid quizId, string token);
    ValidatedAnswerDto ValidateAnswer(string token, Guid quizId, Guid questionId, AnswerAttemptDto answerAttemptDto);
    void CompleteQuizAttempt(string token, Guid quizId);
    void DeleteQuiz(string token, Guid quizId);
}