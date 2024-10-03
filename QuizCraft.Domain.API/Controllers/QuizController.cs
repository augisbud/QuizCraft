using Microsoft.AspNetCore.Mvc;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Controllers;

[ApiController]
public class QuizController(IQuizService quizService) : ControllerBase
{
    [HttpPost]
    [Route("/quiz")]
    public async Task<ActionResult<QuizDto>> CreateQuiz([FromBody] string source)
    {      
        return Ok(await quizService.CreateQuiz(source));
    }

    [HttpGet]
    [Route("/quizzes")]
    public ActionResult<IEnumerable<QuizDto>> RetrieveQuizzes()
    {
        return Ok(quizService.RetrieveQuizzes());
    }
}