namespace QuizCraft.Domain.API.Services;

public interface IPdfExportService
{
    Task<byte[]> GenerateQuizPdfAsync(Guid quizId, string token);
}