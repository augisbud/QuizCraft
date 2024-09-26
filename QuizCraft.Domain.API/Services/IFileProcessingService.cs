using Microsoft.AspNetCore.Http;
using QuizCraft.Domain.API.Models;
using System.Threading.Tasks;

namespace QuizCraft.Domain.API.Services
{
    public interface IFileProcessingService
    {
        Task<FileProcessingResultDto> ProcessFileAsync(IFormFile file);
    }
}
