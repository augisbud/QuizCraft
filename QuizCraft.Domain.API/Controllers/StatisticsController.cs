using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Controllers;

[ApiController]
[Route("statistics")]
public class StatisticsController(IStatisticsService service) : ControllerBase
{
    [Authorize]
    [HttpGet("individual/quizzes/{id}")]
    public ActionResult<QuizAttemptsDto> GetQuizAttemptsForUser(Guid id)
    {
        var token = HttpContext.Request.Headers.Authorization.First()!.Replace("Bearer ", "");

        var result = service.QuizAttemptsForUser(token, id);

        return Ok(result);
    }

    [HttpGet("global")]
    public async Task<ActionResult<IEnumerable<StatisticDto>>> GetGlobalStatistics()
    {
        try
        {
            var stats = await service.GetGlobalStatisticsAsync();

            if (stats == null)
            {
                return StatusCode(500, "Failed to retrieve global statistics.");
            }

            return Ok(stats);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}

