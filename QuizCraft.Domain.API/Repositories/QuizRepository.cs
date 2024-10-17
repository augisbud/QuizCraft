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

    public QuizDto? RetrieveQuizById(Guid id)
    {
        var data = context.Quizzes
            .Where(quiz => quiz.Id == id)
            .ProjectTo<QuizDto>(mapper.ConfigurationProvider)
            .FirstOrDefault();

        return data;
    }

    public IEnumerable<QuizDto> RetrieveQuizzes()
    {
        var data = context.Quizzes
            .ProjectTo<QuizDto>(mapper.ConfigurationProvider);

        return data;
    }
}