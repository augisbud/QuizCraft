using QuizCraft.Domain.API.Models;

namespace QuizCraft.Domain.API.Services;

public interface IQuizService
{
    Task<string> GetQuizNameByIdAsync(Guid quizId);
    Task<Guid> CreateQuizAsync(string source, string token);
    IEnumerable<QuizDto> RetrieveQuizzes(string? token);
    Task<DetailedQuizDto> RetrieveQuestions(Guid quizId, string token);
    ValidatedAnswerDto ValidateAnswer(string token, Guid quizId, Guid questionId, AnswerAttemptDto answerAttemptDto);
    Task CompleteQuizAttempt(string token, Guid quizId);
    Task DeleteQuiz(string token, Guid quizId);
}