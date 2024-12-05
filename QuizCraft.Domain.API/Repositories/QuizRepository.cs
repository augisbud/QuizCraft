using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QuizCraft.Domain.API.Data;
using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Models;

namespace QuizCraft.Domain.API.Repositories;

public class QuizRepository(QuizzesDbContext context, IMapper mapper) : BaseRepository<Quiz>(context, mapper), IQuizRepository
{
    private readonly IMapper _mapper = mapper;

    public Quiz? RetrieveQuizWithQuestionsById(Guid id)
    {
        return _dbSet
            .AsSplitQuery()
            .Include(quiz => quiz.Questions)
            .ThenInclude(question => question.Answers)
            .Include(quiz => quiz.QuizAttempts)
            .ThenInclude(attempt => attempt.QuizAnswerAttempts)
            .FirstOrDefault(quiz => quiz.Id == id);
    }

    public IEnumerable<QuestionDto> RetrieveQuestions(Guid quizId)
    {
        return _context.Set<Question>()
            .Where(q => q.QuizId == quizId)
            .ProjectTo<QuestionDto>(_mapper.ConfigurationProvider);
    }

    public AnswerDto? RetrieveAnswer(Guid quizId, Guid questionId)
    {
        return _context.Set<Answer>()
            .Where(answer => answer.Question.QuizId == quizId && answer.QuestionId == questionId && answer.IsCorrect)
            .ProjectTo<AnswerDto>(_mapper.ConfigurationProvider)
            .FirstOrDefault();
    }
}