using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;

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
        var result = quizService.RetrieveQuizById(id);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("/quizzes/{id}/questions")]
    public ActionResult<IEnumerable<QuestionDto>> GetQuestions(Guid id)
    {
        var result = quizService.RetrieveQuestions(id);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("/quizzes/{quizId}/questions/{questionId}")]
    public ActionResult<AnswerValidationDto> ValidateAnswer(Guid quizId, Guid questionId, [FromBody] AnswerValidationInputDto inputDto)
    {
        var result = quizService.ValidateAnswer(quizId, questionId, inputDto);

        return Ok(result);
    }
}
