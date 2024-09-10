using Microsoft.AspNetCore.Mvc;

namespace QuizCraft.Domain.API.Controllers;

[ApiController]
public class QuizController : ControllerBase
{
    [HttpGet]
    [Route("/quizes")]
    public IActionResult GetQuizes()
    {
        var response = new List<string> { "Quiz 1", "Quiz 2" };
        
        return Ok(response);
    }
}