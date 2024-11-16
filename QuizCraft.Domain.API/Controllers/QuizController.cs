using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;
using System.IdentityModel.Tokens.Jwt;

namespace QuizCraft.Domain.API.Controllers;

[ApiController]
public class QuizController(IQuizService quizService, IFileProcessingService fileProcessingService) : ControllerBase
{
    [Authorize]
    [HttpPost("/quizzes")]
    public async Task<ActionResult<QuizDto>> CreateQuizAsync([FromForm] IFormFile file)
    {
        var source = await fileProcessingService.ProcessFileAsync(file);
        var result = await quizService.CreateQuizAsync(source);

        return Ok(result);
    }

    [HttpGet("/quizzes")]
    public ActionResult<IEnumerable<QuizDto>> GetQuizzes()
    {
        var result = quizService.RetrieveQuizzes();

        return Ok(result);
    }

    [Authorize]
    [HttpGet("/quizzes/{id}")]
    public ActionResult<QuizDto> GetQuizById(Guid id)
    {
        var quiz = quizService.RetrieveQuizById(id);

        return Ok(quiz);
    }

    [Authorize]
    [HttpGet("/quizzes/{id}/questions")]
    public ActionResult<IEnumerable<QuestionDto>> GetQuestions(Guid id)
    {
        var token = HttpContext.Request.Headers.Authorization.First()!.Replace("Bearer ", "");

        var result = quizService.RetrieveQuestions(id, token);

        return Ok(result);
    }


    [Authorize]
    [HttpPost("/quizzes/{quizId}/questions/{questionId}")]
    public async Task<ActionResult<AnswerValidationDto>> ValidateAnswer(Guid quizId, Guid questionId, [FromBody] AnswerValidationInputDto inputDto)
    {
        var token = HttpContext.Request.Headers.Authorization.First()!.Replace("Bearer ", "");

        var result = await quizService.ValidateAnswerAndTrackAttemptAsync(quizId, questionId, inputDto, token);

        return Ok(result);
    }
}
