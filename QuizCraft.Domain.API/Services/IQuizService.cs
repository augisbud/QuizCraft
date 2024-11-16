using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Entities;

namespace QuizCraft.Domain.API.Services;

public interface IQuizService
{
    Task<QuizDto> CreateQuizAsync(string source);
    QuizDto RetrieveQuizById(Guid id);
    IEnumerable<QuestionDto> RetrieveQuestions(Guid quizId, string token);
    IEnumerable<QuizDto> RetrieveQuizzes();
    Task<AnswerValidationDto> ValidateAnswerAndTrackAttemptAsync(Guid quizId, Guid questionId, AnswerValidationInputDto inputDto, string token);
}