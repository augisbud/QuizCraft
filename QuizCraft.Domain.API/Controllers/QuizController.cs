using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Controllers;

[ApiController]
public class QuizController(IQuizService quizService, IFileProcessingService fileProcessingService, IPdfExportService pdfExportService) : ControllerBase
{
    [Authorize]
    [HttpPost("/quizzes")]
    public async Task<ActionResult<Guid>> CreateQuizAsync([FromForm] IFormFile file)
    {
        var token = HttpContext.Request.Headers.Authorization.First()!.Replace("Bearer ", "");

        var source = await fileProcessingService.ProcessFileAsync(file);
        var result = await quizService.CreateQuizAsync(source, token);

        return Ok(result);
    }

    [HttpGet("/quizzes")]
    public ActionResult<IEnumerable<QuizDto>> GetQuizzes()
    {
        var token = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");

        var result = quizService.RetrieveQuizzes(token);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("/quizzes/{id}/questions")]
    public async Task<ActionResult<DetailedQuizDto>> GetDetailedQuizDto(Guid id)
    {
        var token = HttpContext.Request.Headers.Authorization.First()!.Replace("Bearer ", "");

        var result = await quizService.RetrieveQuestions(id, token);

        return Ok(result);
    }


    [Authorize]
    [HttpPost("/quizzes/{quizId}/questions/{questionId}")]
    public ActionResult<ValidatedAnswerDto> ValidateAnswer(Guid quizId, Guid questionId, [FromBody] AnswerAttemptDto inputDto)
    {
        var token = HttpContext.Request.Headers.Authorization.First()!.Replace("Bearer ", "");

        var result = quizService.ValidateAnswer(token, quizId, questionId, inputDto);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("/quizzes/{quizId}")]
    public async Task<ActionResult> CompleteQuizAttempt(Guid quizId)
    {
        var token = HttpContext.Request.Headers.Authorization.First()!.Replace("Bearer ", "");

        await quizService.CompleteQuizAttempt(token, quizId);

        return Ok();
    }

    [Authorize]
    [HttpDelete("/quizzes/{quizId}")]
    public async Task<ActionResult> DeleteQuiz(Guid quizId)
    {
        var token = HttpContext.Request.Headers.Authorization.First()!.Replace("Bearer ", "");

        await quizService.DeleteQuiz(token, quizId);

        return Ok();
    }

    [Authorize]
    [HttpGet("{quizId}/export-pdf")]
    [Produces("application/pdf")]
    public async Task<IActionResult> ExportQuizToPdf(Guid quizId)
    {
        var token = HttpContext.Request.Headers.Authorization.First()!.Replace("Bearer ", "");

        var pdfBytes = await pdfExportService.GenerateQuizPdfAsync(quizId, token);
        var quizName = await quizService.GetQuizNameByIdAsync(quizId);

        var encodedQuizName = Uri.EscapeDataString(quizName);
        Response.Headers["Content-Disposition"] = $"attachment; filename=\"{encodedQuizName}.pdf\"";
        Response.Headers["Content-Type"] = "application/pdf";

        return File(pdfBytes, "application/pdf", $"{encodedQuizName}.pdf");
    }
}
