using Microsoft.AspNetCore.Mvc;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Controllers;

[ApiController]
public class QuizController(IQuizService quizService) : ControllerBase
{
    [HttpGet]
    [Route("/quizes")]
    public async Task<ActionResult<QuestionDto>> GetQuizes()
    {      
        return Ok(await quizService.GenerateQuiz());
    }
}