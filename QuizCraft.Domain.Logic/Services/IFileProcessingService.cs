namespace QuizCraft.Domain.Logic.Services;

public interface IFileProcessingService
{
    Task<string> ProcessFileAsync(IFormFile file);
}