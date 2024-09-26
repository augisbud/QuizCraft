using Microsoft.AspNetCore.Mvc;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Controllers;

[ApiController]
public class FileUploadController(IFileProcessingService fileProcessingService) : ControllerBase
{
    [HttpPost]
    [Route("/upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        Console.WriteLine($"Uploaded file extension: {fileExtension}");

        if (fileExtension != ".txt" && fileExtension != ".docx" && fileExtension != ".pdf")
            return BadRequest("Invalid file type. Only .txt, .docx and .pdf files are allowed.");

        //calling file processing
        var result = await fileProcessingService.ProcessFileAsync(file);

        if (result == null)
        {
            return BadRequest("File processing failed.");
        }

        return Ok(result);
    }
}