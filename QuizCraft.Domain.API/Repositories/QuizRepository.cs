using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
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

    public IEnumerable<QuizForTransferDto> RetrieveQuizzes()
    {
        var data = context.Quizzes
            .ProjectTo<QuizForTransferDto>(mapper.ConfigurationProvider);

        return data;
    }

    public Quiz? RetrieveQuizById(Guid id)
    {
        var data = context.Quizzes
            .Include(quiz => quiz.Questions)
            .ThenInclude(question => question.Answers)
            .Where(quiz => quiz.Id == id)
            .FirstOrDefault();

        return data;
    }

    public Quiz? RetrieveQuizWithQuestionsById(Guid id)
    {
        var data = context.Quizzes
            .AsSplitQuery()
            .Include(quiz => quiz.Questions)
            .ThenInclude(question => question.Answers)
            .Include(quiz => quiz.QuizAttempts)
            .ThenInclude(attempt => attempt.QuizAnswerAttempts)
            .Where(quiz => quiz.Id == id)
            .FirstOrDefault();

        return data;
    }

    public IEnumerable<QuestionDto> RetrieveQuestions(Guid quizId)
    {
        return context.Questions
            .Where(q => q.QuizId == quizId)
            .ProjectTo<QuestionDto>(mapper.ConfigurationProvider);
    }

    public AnswerDto? RetrieveAnswer(Guid quizId, Guid questionId)
    {
        var data = context.Answers
            .Where(answer => answer.Question.QuizId == quizId && answer.QuestionId == questionId && answer.IsCorrect)
            .ProjectTo<AnswerDto>(mapper.ConfigurationProvider)
            .FirstOrDefault();

        return data;
    }

    public QuizAttempt? RetrieveQuizAttempt(Guid quizId, string email)
    {
        return context.QuizAttempts
            .Include(attempt => attempt.QuizAnswerAttempts)
            .Where(attempt => attempt.QuizId == quizId && attempt.UserEmail == email && !attempt.IsCompleted)
            .FirstOrDefault();
    }

    public IEnumerable<QuizAttempt> RetrieveQuizAttempts(Guid quizId, string email)
    {
        return context.QuizAttempts
            .Include(attempt => attempt.QuizAnswerAttempts)
            .ThenInclude(answerAttempt => answerAttempt.Answer)
            .Where(attempt => attempt.QuizId == quizId && attempt.UserEmail == email)
            .OrderBy(attempt => attempt.StartedAt);
    }

    public QuizAttempt CreateQuizAttempt(Guid quizId, string email)
    {
        var result = context.QuizAttempts.Add(new QuizAttempt
        {
            QuizId = quizId,
            UserEmail = email,
            IsCompleted = false
        });

        context.SaveChanges();
        
        return result.Entity;
    }

    public IEnumerable<QuizAnswerAttempt> RetrieveQuizAnswerAttempt(Guid attemptId)
    {
        return context.QuizAnswerAttempts
            .Where(attempt => attempt.QuizAttemptId == attemptId);
    }

    public QuizAnswerAttempt CreateQuizAnswerAttempt(Guid attemptId, Guid questionId, Guid answerId)
    {
        var result = context.QuizAnswerAttempts.Add(new QuizAnswerAttempt
        {
            QuizAttemptId = attemptId,
            QuestionId = questionId,
            AnswerId = answerId
        });

        context.SaveChanges();

        return result.Entity;
    }

    public void DeleteQuiz(Quiz quiz)
    {
        context.Quizzes.Remove(quiz);

        context.SaveChanges();
    }

    public bool SaveChanges()
    {
        return context.SaveChanges() > 0;
    }

    public async Task<int> GetTotalUsersAsync()
    {
        return await context.QuizAttempts
            .Select(q => q.UserEmail)
            .Distinct()
            .CountAsync();
    }

    public async Task<int> GetTotalQuizzesCreatedAsync()
    {
        return await context.Quizzes.CountAsync();
    }

    public async Task<double> GetAverageQuizzesTakenPerUserAsync()
    {
        var totalUsers = await GetTotalUsersAsync();
        var totalAttempts = await context.QuizAttempts.CountAsync();

        return totalUsers == 0 ? 0 : (double)totalAttempts / totalUsers;
    }
}