using Microsoft.AspNetCore.Http;
using QuizCraft.Domain.API.Models;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using Xceed.Words.NET; // For .docx
using iText.Kernel.Pdf; // For .pdf
using iText.Kernel.Pdf.Canvas.Parser; // For .pdf

namespace QuizCraft.Domain.API.Services
{
    public class FileProcessingService : IFileProcessingService
    {
        public async Task<FileProcessingResultDto> ProcessFileAsync(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0; // Reset stream position

                string processedData;

                switch (Path.GetExtension(file.FileName).ToLower())
                {
                    case ".txt":
                        processedData = await ProcessTextFileAsync(stream);
                        break;

                    case ".docx":
                        processedData = await ProcessDocxFileAsync(stream);
                        break;

                    case ".pdf":
                        processedData = await ProcessPdfFileAsync(stream);
                        break;

                    default:
                        throw new NotSupportedException("File type not supported.");
                }

                return new FileProcessingResultDto
                {
                    ProcessedData = processedData
                };
            }
        }

        private async Task<string> ProcessTextFileAsync(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private async Task<string> ProcessDocxFileAsync(Stream stream)
        {
            using (var document = DocX.Load(stream))
            {
                var text = new StringBuilder();
                foreach (var paragraph in document.Paragraphs)
                {
                    text.Append(paragraph.Text);
                    text.AppendLine();
                }
                return await Task.FromResult(text.ToString());
            }
        }

        private async Task<string> ProcessPdfFileAsync(Stream stream)
        {
            using (var pdfReader = new PdfReader(stream))
            {
                using (var pdfDocument = new PdfDocument(pdfReader))
                {
                    var text = new StringBuilder();
                    for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                    {
                        var page = pdfDocument.GetPage(i);
                        text.Append(PdfTextExtractor.GetTextFromPage(page));
                    }
                    return await Task.FromResult(text.ToString());
                }
            }
        }
    }
}
