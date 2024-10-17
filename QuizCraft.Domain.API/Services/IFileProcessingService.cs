namespace QuizCraft.Domain.API.Services;

public interface IFileProcessingService
{
    Task<string> ProcessFileAsync(IFormFile file);
}