using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Entities;

namespace QuizCraft.Domain.API.Services;

public interface IQuizService
{
    Task<QuizDto> CreateQuizAsync(string source);
    QuizDto RetrieveQuizById(Guid id);
    IEnumerable<QuestionDto> RetrieveQuestions(Guid quizId);
    AnswerValidationDto ValidateAnswer(Guid quizId, Guid questionId, AnswerValidationInputDto inputDto);
    IEnumerable<QuizDto> RetrieveQuizzes();
    Task<AnswerValidationDto> ValidateAnswerAndTrackAttemptAsync(Guid quizId, Guid questionId, AnswerValidationInputDto inputDto, string userEmail);
    Task<QuizAnswerAttempt> CreateQuizAnswerAttemptAsync(QuizAnswerAttempt attempt);
    IEnumerable<QuizAnswerAttempt> RetrieveQuizAnswerAttempts(Guid quizId);
    IEnumerable<QuizAnswerAttempt> RetrieveAttemptsForQuestion(Guid quizId, Guid questionId);
}