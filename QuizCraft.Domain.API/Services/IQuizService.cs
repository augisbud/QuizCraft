using QuizCraft.Domain.API.Models;

namespace QuizCraft.Domain.API.Services;

public interface IQuizService
{
    Task<QuizDto> CreateQuizAsync(string source);
    QuizDto RetrieveQuizById(Guid id);
    IEnumerable<QuestionDto> RetrieveQuestions(Guid quizId);
    AnswerValidationDto ValidateAnswer(Guid quizId, Guid questionId, AnswerValidationInputDto inputDto);
    IEnumerable<QuizDto> RetrieveQuizzes();
}