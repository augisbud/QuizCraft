using Microsoft.AspNetCore.Mvc;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Controllers;

[ApiController]
public class QuizController(IQuizService quizService, IFileProcessingService fileProcessingService) : ControllerBase
{
    [HttpPost("/quizzes")]
    public async Task<ActionResult<QuizDto>> CreateQuizAsync([FromForm] IFormFile file)
    {
        var source = await fileProcessingService.ProcessFileAsync(file);

        return Ok(await quizService.CreateQuizAsync(source));
    }

    [HttpGet("/quizzes")]
    public ActionResult<IEnumerable<QuizDto>> GetQuizzes()
    {
        return Ok(quizService.RetrieveQuizzes());
    }

    [HttpGet("/quizzes/{id}")]
    public ActionResult<QuizDto> GetQuizById(Guid id)
    {
        return Ok(quizService.RetrieveQuizById(id));
    }

    [HttpGet("/quizzes/{id}/questions")]
    public ActionResult<IEnumerable<QuestionDto>> GetQuestions(Guid id)
    {
        return Ok(quizService.RetrieveQuestions(id));
    }

    [HttpPost("/quizzes/{quizId}/questions/{questionId}")]
    public ActionResult<AnswerValidationDto> ValidateAnswer(Guid quizId, Guid questionId, [FromBody] AnswerValidationInputDto inputDto)
    {
        return Ok(quizService.ValidateAnswer(quizId, questionId, inputDto));
    }
}
