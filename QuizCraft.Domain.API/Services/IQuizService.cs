using QuizCraft.Domain.API.Models;

namespace QuizCraft.Domain.API.Services;

public interface IQuizService
{
    Task<QuizDto> CreateQuiz(string source);
    IEnumerable<QuizDto> RetrieveQuizzes();
}