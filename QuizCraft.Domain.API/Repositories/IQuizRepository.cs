using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Models;

namespace QuizCraft.Domain.API.Repositories;

public interface IQuizRepository
{
    Task<Quiz> CreateQuizAsync(Quiz quiz);
    Task<IEnumerable<QuizDto>> RetrieveQuizzesAsync();
}