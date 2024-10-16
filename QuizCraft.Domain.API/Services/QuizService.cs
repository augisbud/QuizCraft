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

        // 3. Property usage in class
        var jsonString = response.Candidates[0].Content.Parts[0].Text.CleanJsonString();

        var data = JsonSerializer.Deserialize<List<QuestionDto>>(jsonString) ?? throw new InsufficientDataException();

        var quiz = await repository.CreateQuizAsync(new Quiz
        {
            Questions = mapper.Map<List<Question>>(data)
        });

        return mapper.Map<QuizDto>(quiz);
    }

    public QuizDto RetrieveQuizById(Guid id)
    {
        var quiz = repository.RetrieveQuizById(id) ?? throw new QuizNotFoundException(id);

        return quiz;
    }

    public IEnumerable<QuizDto> RetrieveQuizzes()
    {       
        var quizzes = repository.RetrieveQuizzes();
        var data = new List<QuizDto>();

        // 8. Boxing and unboxing
        var quizScores = new ArrayList();
        foreach(var quiz in quizzes)
        {
            quizScores.Add(new QuizScore(quiz.Id, quiz.Questions.Count * 10));
        }

        foreach(var obj in quizScores)
        {
            var quizScore = (QuizScore) obj;

            // 3. Property usage in struct
            // 9. LINQ to Objects usage (methods or queries)
            data.Add(quizzes.Where(quiz => quizScore.Score >= 50 && quiz.Id == quizScore.QuizId).First());
        }

        return data;
    }
}