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

    public IEnumerable<QuizDto> RetrieveQuizzes()
    {
        var data = context.Quizzes
            .ProjectTo<QuizDto>(mapper.ConfigurationProvider);

        return data;
    }

    public QuizDto? RetrieveQuizById(Guid id)
    {
        var data = context.Quizzes
            .Where(quiz => quiz.Id == id)
            .ProjectTo<QuizDto>(mapper.ConfigurationProvider)
            .FirstOrDefault();

        return data;
    }

    public IEnumerable<QuestionDto> RetrieveQuestions(Guid quizId)
    {
        var data = context.Questions
            .Where(question => question.QuizId == quizId)
            .ProjectTo<QuestionDto>(mapper.ConfigurationProvider);

        return data;
    }

    public AnswerDto? RetrieveAnswer(Guid quizId, Guid questionId)
    {
        var data = context.Answers
            .Where(answer => answer.IsCorrect && answer.QuestionId == questionId && answer.Question.QuizId == quizId)
            .ProjectTo<AnswerDto>(mapper.ConfigurationProvider)
            .FirstOrDefault();

        return data;
    }
}