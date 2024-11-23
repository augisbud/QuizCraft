using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Controllers;

[ApiController]
[Authorize]
public class StatisticsController(IStatisticsService service) : ControllerBase
{
    [HttpGet("/statistics/individual/quizzes/{id}")]
    public async Task<ActionResult<QuizAttemptsDto>> GetQuizAttemptsForUser(Guid id)
    {
        var token = HttpContext.Request.Headers.Authorization.First()!.Replace("Bearer ", "");

        var result = await service.QuizAttemptsForUser(token, id);

        return Ok(result);
    }
}
