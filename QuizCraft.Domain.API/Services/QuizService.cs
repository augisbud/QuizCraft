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
using System.IdentityModel.Tokens.Jwt;

namespace QuizCraft.Domain.API.Services;

public class QuizService(IGeminiAPIClient geminiAPIClient, IMapper mapper, IQuizRepository repository) : IQuizService
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

        return repository.RetrieveQuizById(id) ?? throw new QuizNotFoundException(id);
    }

    public IEnumerable<QuestionDto> RetrieveQuestions(Guid quizId, string token)
    {
        // TODO: throw error, when questions are not found for a given quiz.

        return repository.RetrieveQuestions(quizId, DecodeJwtToken(token));
    }

    public IEnumerable<QuizDto> RetrieveQuizzes()
    {       
        var quizzes = repository.RetrieveQuizzes();

        return quizzes;
    }

    public async Task<AnswerValidationDto> ValidateAnswerAndTrackAttemptAsync(Guid quizId, Guid questionId, AnswerValidationInputDto inputDto, string token)
    {
        var answer = repository.RetrieveAnswer(quizId, questionId) ?? throw new AnswerNotFoundException(questionId);

        await repository.CreateQuizAnswerAttemptAsync(new QuizAnswerAttempt
        {
            QuizId = quizId,
            QuestionId = questionId,
            AttemptedAnswer = inputDto.Text,
            IsCorrect = answer.Text == inputDto.Text,
            UserEmail = DecodeJwtToken(token),
            AttemptedAt = DateTime.UtcNow
        });

        return new AnswerValidationDto
        {
            Selected = inputDto.Text,
            IsCorrect = answer.Text == inputDto.Text,
            CorrectAnswer = answer
        };
    }

    private static string DecodeJwtToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        return jwtToken.Claims.First(c => c.Type == "email").Value;
    }
}