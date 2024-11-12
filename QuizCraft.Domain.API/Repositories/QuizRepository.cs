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

    public IEnumerable<QuestionDto> RetrieveQuestions(Guid quizId, string userEmail)
    {
        // Retrieve the IDs of questions already answered by the user for the specific quiz
        var answeredQuestionIds = context.QuizAnswerAttempts
            .Where(qa => qa.QuizId == quizId && qa.UserEmail == userEmail)
            .Select(qa => qa.QuestionId)
            .ToList();

        // Retrieve unanswered questions for the given quiz
        var unansweredQuestions = context.Questions
            .Where(q => q.QuizId == quizId && !answeredQuestionIds.Contains(q.Id))
            .ProjectTo<QuestionDto>(mapper.ConfigurationProvider)
            .ToList();

        return unansweredQuestions;
    }

    public AnswerDto? RetrieveAnswer(Guid quizId, Guid questionId)
    {
        var data = context.Answers
            .Where(answer => answer.IsCorrect && answer.QuestionId == questionId && answer.Question.QuizId == quizId)
            .ProjectTo<AnswerDto>(mapper.ConfigurationProvider)
            .FirstOrDefault();

        return data;
    }

    public async Task<QuizAnswerAttempt> CreateQuizAnswerAttemptAsync(QuizAnswerAttempt attempt)
    {
        var result = await context.QuizAnswerAttempts.AddAsync(attempt);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public IEnumerable<QuizAnswerAttempt> RetrieveQuizAnswerAttempts(Guid quizId)
    {
        var attempts = context.QuizAnswerAttempts
            .Where(attempt => attempt.QuizId == quizId)
            .Include(attempt => attempt.Question)
            .ToList();
        return attempts;
    }

    public IEnumerable<QuizAnswerAttempt> RetrieveAttemptsForQuestion(Guid quizId, Guid questionId)
    {
        var attempts = context.QuizAnswerAttempts
            .Where(attempt => attempt.QuizId == quizId && attempt.QuestionId == questionId)
            .ToList();
        return attempts;
    }
}