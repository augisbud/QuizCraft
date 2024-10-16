using AutoMapper;
using AutoMapper.QueryableExtensions;
using QuizCraft.Domain.API.Data;
using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizCraft.Domain.API.Repositories;

public class QuizRepository(QuizzesDbContext context, IMapper mapper) : IQuizRepository
{
    public async Task<Quiz> CreateQuizAsync(Quiz quiz)
    {
        var result = await context.Quizzes.AddAsync(quiz);
        
        await context.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<IEnumerable<QuizDto>> RetrieveQuizzesAsync()
    {
        var data = await context.Quizzes
            .ProjectTo<QuizDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return data;
    }

}