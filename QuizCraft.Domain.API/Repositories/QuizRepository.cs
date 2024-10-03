using AutoMapper;
using AutoMapper.QueryableExtensions;
using QuizCraft.Domain.API.Data;
using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Models;

namespace QuizCraft.Domain.API.Repositories;

public class QuizRepository(QuizzesDbContext context, IMapper mapper) : IQuizRepository
{
    public async Task<Quiz> CreateQuizAsync(Quiz quiz)
    {
        var result = await context.Quizzes.AddAsync(quiz);
        
        await context.SaveChangesAsync();

        return result.Entity;
    }

    public IEnumerable<QuizDto> RetrieveQuizzes()
    {
        var data = context.Quizzes.ProjectTo<QuizDto>(mapper.ConfigurationProvider).ToList();

        return data;
    }
}