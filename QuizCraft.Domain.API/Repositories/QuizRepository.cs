using QuizCraft.Domain.API.Data;
using QuizCraft.Domain.API.Entities;

namespace QuizCraft.Domain.API.Repositories;

public class QuizRepository(QuizzesDbContext context) : IQuizRepository
{
    public async Task<Quiz> AddQuizAsync(Quiz quiz)
    {
        var result = await context.Quizzes.AddAsync(quiz);
        
        await context.SaveChangesAsync();

        return result.Entity;
    }

    public IEnumerable<Quiz> GetQuizzes()
    {
        return [ .. context.Quizzes ];
    }
}