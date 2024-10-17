using System.Text;
using Xceed.Words.NET;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using QuizCraft.Domain.API.Exceptions;

namespace QuizCraft.Domain.API.Services;

public class FileProcessingService : IFileProcessingService
{
    public async Task<string> ProcessFileAsync(IFormFile file)
    {
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        string processedData = fileExtension switch
        {
            ".txt" => await ProcessTextFileAsync(file),
            ".docx" => await ProcessDocxFileAsync(file),
            ".pdf" => await ProcessPdfFileAsync(file),
            _ => throw new InvalidFileExtensionException(fileExtension)
        };

        return processedData;
    }

    private static async Task<string> ProcessTextFileAsync(IFormFile file)
    {
        using var reader = new StreamReader(ProcessStream(file), Encoding.UTF8);

        return await reader.ReadToEndAsync();
    }

    private static async Task<string> ProcessDocxFileAsync(IFormFile file)
    {
        using var document = DocX.Load(ProcessStream(file));
        
        var text = new StringBuilder();
        foreach (var paragraph in document.Paragraphs)
        {
            text.Append(paragraph.Text);
            text.AppendLine();
        }
        
        return await Task.FromResult(text.ToString());
    }

    private static async Task<string> ProcessPdfFileAsync(IFormFile file)
    {
        using var pdfReader = new PdfReader(ProcessStream(file));
        using var pdfDocument = new PdfDocument(pdfReader);

        var text = new StringBuilder();
        for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
        {
            var page = pdfDocument.GetPage(i);
            text.Append(PdfTextExtractor.GetTextFromPage(page));
        }
        
        return await Task.FromResult(text.ToString());
    }

    // 7. Using a stream to load data (can be from file, web service, socket etc.)
    private static MemoryStream ProcessStream(IFormFile file)
    {
        var stream = new MemoryStream();
        file.CopyTo(stream);
        stream.Position = 0;

        return stream;
    }
}