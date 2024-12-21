namespace QuizCraft.Domain.Logic.Services;

public interface IPdfExportService
{
    Task<byte[]> GenerateQuizPdfAsync(Guid quizId, string token);
}