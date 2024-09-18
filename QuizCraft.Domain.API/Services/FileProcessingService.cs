using Microsoft.AspNetCore.Http;
using QuizCraft.Domain.API.Models;
using System.IO;
using System.Threading.Tasks;

namespace QuizCraft.Domain.API.Services
{
    public class FileProcessingService : IFileProcessingService
    {
        public async Task<FileProcessingResultDto> ProcessFileAsync(IFormFile file)
        {
            //TODO file processing logic here.

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var processedData = System.Text.Encoding.UTF8.GetString(stream.ToArray());

                return new FileProcessingResultDto
                {
                    ProcessedData = processedData
                };
            }
        }
    }
}
