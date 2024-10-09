using Microsoft.AspNetCore.Mvc;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Controllers;

[ApiController]
[Route("quiz")]
public class QuizController(IQuizService quizService) : ControllerBase
{
    [HttpPost]
    [Route("generate")]
    public async Task<ActionResult<QuestionDto>> GenerateQuiz([FromBody] FileProcessingResultDto request)
    {
        if (string.IsNullOrEmpty(request.ProcessedData))
        {
            return BadRequest("Processed data is required.");
        }

        var quiz = await quizService.GenerateQuiz(request.ProcessedData);
        return Ok(quiz);
    }

}