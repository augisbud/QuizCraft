using Microsoft.AspNetCore.Mvc;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Controllers;

[ApiController]
[Route("quizzes")]
public class QuizController : ControllerBase
{
    private readonly IQuizService quizService;
    private readonly IFileProcessingService fileProcessingService;

    public QuizController(IQuizService quizService, IFileProcessingService fileProcessingService)
    {
        this.quizService = quizService;
        this.fileProcessingService = fileProcessingService;
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<QuizDto>>> GenerateOrRetrieveQuizzes([FromForm] IFormFile? file)
    {        
        if (file != null && file.Length > 0)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (fileExtension != ".txt" && fileExtension != ".docx" && fileExtension != ".pdf")
            {
                return BadRequest("Invalid file type. Only .txt, .docx, and .pdf files are allowed.");
            }

            var result = await fileProcessingService.ProcessFileAsync(file);
            if (result == null)
            {
                return BadRequest("File processing failed.");
            }

            var source = result.ProcessedData;
            var generatedQuiz = await quizService.CreateQuiz(source);
            return Ok(generatedQuiz);
        }

        var quizzes = await quizService.RetrieveQuizzesAsync();
        return Ok(quizzes);
    }
}
