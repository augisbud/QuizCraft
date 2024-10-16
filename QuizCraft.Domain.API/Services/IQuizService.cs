using QuizCraft.Domain.API.Models;

namespace QuizCraft.Domain.API.Services;

public interface IQuizService
{
    Task<QuizDto> CreateQuiz(string source);
    Task<IEnumerable<QuizDto>> RetrieveQuizzesAsync();
}