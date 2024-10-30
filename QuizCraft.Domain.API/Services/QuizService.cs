using System.Collections;
using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizCraft.Domain.API.APIClients;
using QuizCraft.Domain.API.Constants;
using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Exceptions;
using QuizCraft.Domain.API.Extensions;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Repositories;
using QuizCraft.Domain.API.Data;

namespace QuizCraft.Domain.API.Services;

public class QuizService(IGeminiAPIClient geminiAPIClient, IMapper mapper, IQuizRepository repository, QuizzesDbContext context) : IQuizService
{
    public async Task<QuizDto> CreateQuizAsync(string source)
    {
        // 4. Named and optional argument usage
        var response = await geminiAPIClient.PostAsync(GeminiAPITemplates.GeneratePrompt(numberOfQuestions: 10, source: source));

        var jsonString = response.Candidates[0].Content.Parts[0].Text.CleanJsonString();

        var data = JsonSerializer.Deserialize<QuizDtoForCreation>(jsonString) ?? throw new InsufficientDataException();

        var mappedData = mapper.Map<Quiz>(data);

        var quiz = await repository.CreateQuizAsync(mappedData);

        return mapper.Map<QuizDto>(quiz);
    }

    public QuizDto RetrieveQuizById(Guid id)
    {
        // 8. Boxing and unboxing
        object boxedQuiz = repository.RetrieveQuizById(id) ?? throw new QuizNotFoundException(id);
        var unboxedQuiz = (QuizDto) boxedQuiz;

        return unboxedQuiz;
    }

    public IEnumerable<QuestionDto> RetrieveQuestions(Guid quizId)
    {
        // TODO: throw error, when questions are not found for a given quiz.
        var questions = repository.RetrieveQuestions(quizId);
        
        // 8. Boxing and unboxing
        var boxedQuestions = (object) questions;
        var unboxedQuestions = (IEnumerable<Question>) boxedQuestions;

        // 6. Iterating through collection the right way
        var data = unboxedQuestions.Select(q => new QuestionDto
        {
            Id = q.Id,
            Text = q.Text,
            Answers = q.Answers.Select(a => a.Text).ToList()
        });

        return data;
    }

    public AnswerValidationDto ValidateAnswer(Guid quizId, Guid questionId, AnswerValidationInputDto inputDto)
    {
        // TODO: store user progress.
        var answer = repository.RetrieveAnswer(quizId, questionId) ?? throw new AnswerNotFoundException(questionId);

        return new()
        {
            Selected = inputDto.Text,
            IsCorrect = answer.Text == inputDto.Text,
            CorrectAnswer = answer
        };
    }

    public IEnumerable<QuizDto> RetrieveQuizzes()
    {       
        var quizzes = repository.RetrieveQuizzes();

        return quizzes;
    }

    public async Task<AnswerValidationDto> ValidateAnswerAndTrackAttemptAsync(Guid quizId, Guid questionId, AnswerValidationInputDto inputDto, string userEmail)
    {
        var answer = await context.Answers
            .FirstOrDefaultAsync(a => a.QuestionId == questionId && a.Question.QuizId == quizId && a.Text == inputDto.Text);

        if (answer == null)
        {
            throw new AnswerNotFoundException(questionId);
        }

        var attempt = new QuizAnswerAttempt
        {
            QuizId = quizId,
            QuestionId = questionId,
            AttemptedAnswer = inputDto.Text,
            IsCorrect = answer.IsCorrect,
            UserEmail = userEmail,
            AttemptedAt = DateTime.UtcNow
        };

        context.QuizAnswerAttempts.Add(attempt);
        await context.SaveChangesAsync();

        var correctAnswerDto = new AnswerDto(answer.Text, answer.IsCorrect);

        return new AnswerValidationDto
        {
            Selected = inputDto.Text,
            IsCorrect = answer.IsCorrect,
            CorrectAnswer = correctAnswerDto
        };
    }

    public async Task<QuizAnswerAttempt> CreateQuizAnswerAttemptAsync(QuizAnswerAttempt attempt)
    {
        return await repository.CreateQuizAnswerAttemptAsync(attempt);
    }

    public IEnumerable<QuizAnswerAttempt> RetrieveQuizAnswerAttempts(Guid quizId)
    {
        return repository.RetrieveQuizAnswerAttempts(quizId);
    }

    public IEnumerable<QuizAnswerAttempt> RetrieveAttemptsForQuestion(Guid quizId, Guid questionId)
    {
        return repository.RetrieveAttemptsForQuestion(quizId, questionId);
    }
}