using QuizCraft.Domain.API.Entities;

namespace QuizCraft.Domain.API.Repositories;

public interface IQuizRepository
{
    Task<Quiz> AddQuizAsync(Quiz quiz);
    IEnumerable<Quiz> GetQuizzes();
}