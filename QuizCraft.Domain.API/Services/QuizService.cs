using System.Collections;
using System.Text.Json;
using AutoMapper;
using QuizCraft.Domain.API.APIClients;
using QuizCraft.Domain.API.Constants;
using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Exceptions;
using QuizCraft.Domain.API.Extensions;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Repositories;

namespace QuizCraft.Domain.API.Services;

public class QuizService(IGeminiAPIClient geminiAPIClient, IMapper mapper, IQuizRepository repository) : IQuizService
{
    public async Task<QuizDto> CreateQuizAsync(string source)
    {
        var response = await geminiAPIClient.PostAsync(GeminiAPITemplates.GeneratePrompt(source));

        var jsonString = response.Candidates[0].Content.Parts[0].Text.CleanJsonString();

        var data = JsonSerializer.Deserialize<QuizDtoForCreation>(jsonString) ?? throw new InsufficientDataException();

        var quiz = await repository.CreateQuizAsync(mapper.Map<Quiz>(data));

        return mapper.Map<QuizDto>(quiz);
    }

    public QuizDto RetrieveQuizById(Guid id)
    {
        var quiz = repository.RetrieveQuizById(id) ?? throw new QuizNotFoundException(id);

        return quiz;
    }

    public IEnumerable<QuestionDto> RetrieveQuestions(Guid quizId)
    {
        var questions = repository.RetrieveQuestions(quizId);

        // TODO: throw error, when questions are not found for a given quiz.

        return questions;
    }

    public AnswerValidationDto ValidateAnswer(Guid quizId, Guid questionId, AnswerValidationInputDto inputDto)
    {
        var answer = repository.RetrieveAnswer(quizId, questionId) ?? throw new AnswerNotFoundException(questionId);

        // TODO: store user progress.

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
}