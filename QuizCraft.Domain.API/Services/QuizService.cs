using System.Text.Json;
using AutoMapper;
using QuizCraft.Domain.API.APIClients;
using QuizCraft.Domain.API.Constants;
using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Exceptions;
using QuizCraft.Domain.API.Extensions;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Repositories;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;

namespace QuizCraft.Domain.API.Services;

public class QuizService(IGeminiAPIClient geminiAPIClient, IMapper mapper, IQuizRepository repository, JwtSecurityTokenHandler jwtSecurityTokenHandler) : IQuizService
{
    public async Task<Guid> CreateQuizAsync(string source, string token)
    {
        var response = await geminiAPIClient.PostAsync(GeminiAPITemplates.GeneratePrompt(source));

        var jsonString = response.Candidates[0].Content.Parts[0].Text.CleanJsonString();

        var data = JsonSerializer.Deserialize<QuizDtoForCreation>(jsonString) ?? throw new InsufficientDataException();

        var mappedData = mapper.Map<Quiz>(data);
        mappedData.CreatedBy = token.RetrieveEmail(jwtSecurityTokenHandler);

        var quiz = await repository.CreateQuizAsync(mappedData);

        return quiz.Id;
    }

    public IEnumerable<QuizDto> RetrieveQuizzes(string? token)
    {
        var quizzes = repository.RetrieveQuizzes().ToList();

        if(token != null)
            quizzes.Where(x => x.CreatedBy == token.RetrieveEmail(jwtSecurityTokenHandler)).ToList().ForEach(quiz => quiz.IsOwner = true);

        return quizzes;
    }

    public DetailedQuizDto RetrieveQuestions(Guid quizId, string token)
    {
        var quiz = repository.RetrieveQuizWithQuestionsById(quizId) ?? throw new QuizNotFoundException(quizId);

        if(quiz.Questions.Count == 0)
            throw new QuestionsNotFoundException(quizId);

        var currentQuestionId = quiz.Questions.First().Id;

        var quizAttempt = quiz.QuizAttempts.FirstOrDefault(x => !x.IsCompleted && x.UserEmail == token.RetrieveEmail(jwtSecurityTokenHandler));
        if(quizAttempt != null)
        {
            var answeredQuestions = quizAttempt.QuizAnswerAttempts.Select(x => x.QuestionId);
            
            currentQuestionId = quiz.Questions.Where(x => !answeredQuestions.Contains(x.Id)).Select(x => x.Id).FirstOrDefault(Guid.Empty);
        }

        if(currentQuestionId == Guid.Empty) 
        {
            CompleteQuizAttempt(token, quizId);

            currentQuestionId = quiz.Questions.First().Id;
        }
            
        return new()
        {
            Id = quiz.Id,
            Title = quiz.Title,
            CurrentQuestionId = currentQuestionId,
            Questions = mapper.Map<IEnumerable<QuestionDto>>(quiz.Questions)
        };
    }

    public ValidatedAnswerDto ValidateAnswer(string token, Guid quizId, Guid questionId, AnswerAttemptDto answerAttemptDto)
    {
        var answer = repository.RetrieveAnswer(quizId, questionId) ?? throw new AnswerNotFoundException(questionId);

        var quizAttempt = repository.RetrieveQuizAttempt(quizId, token.RetrieveEmail(jwtSecurityTokenHandler)) ?? repository.CreateQuizAttempt(quizId, token.RetrieveEmail(jwtSecurityTokenHandler));

        repository.CreateQuizAnswerAttempt(quizAttempt.Id, questionId, answerAttemptDto.AnswerId);

        return new()
        {
            SelectedAnswer = answerAttemptDto,
            CorrectAnswer = answer
        };
    }

    public void CompleteQuizAttempt(string token, Guid quizId)
    {
        var quizAttempt = repository.RetrieveQuizAttempt(quizId, token.RetrieveEmail(jwtSecurityTokenHandler)) ?? repository.CreateQuizAttempt(quizId, token.RetrieveEmail(jwtSecurityTokenHandler));

        quizAttempt.IsCompleted = true;

        repository.SaveChanges();
    }

    public void DeleteQuiz(string token, Guid quizId)
    {
        var quiz = repository.RetrieveQuizById(quizId) ?? throw new QuizNotFoundException(quizId);

        if(quiz.CreatedBy != token.RetrieveEmail(jwtSecurityTokenHandler))
            throw new UnauthorizedAccessException();

        repository.DeleteQuiz(quiz);

        repository.SaveChanges();
    }
}