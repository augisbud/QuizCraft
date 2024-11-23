using System.Text;
using System.Collections.Concurrent;
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
        var processedData = fileExtension switch
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

        var paragraphs = document.Paragraphs;
        var paragraphTexts = new ConcurrentDictionary<int, string>();

        Parallel.For(0, paragraphs.Count, i =>
        {
            paragraphTexts[i] = paragraphs[i].Text;
        });

        var orderedTexts = paragraphTexts.OrderBy(kv => kv.Key).Select(kv => kv.Value);
        var text = string.Join(Environment.NewLine, orderedTexts);

        return await Task.FromResult(text);
    }

    private static async Task<string> ProcessPdfFileAsync(IFormFile file)
    {
        using var pdfReader = new PdfReader(ProcessStream(file));
        using var pdfDocument = new PdfDocument(pdfReader);

        int pageCount = pdfDocument.GetNumberOfPages();
        var pageTexts = new ConcurrentDictionary<int, string>();

        Parallel.For(1, pageCount + 1, i =>
        {
            var pageText = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(i));
            pageTexts[i] = pageText;
        });

        var orderedTexts = pageTexts.OrderBy(kv => kv.Key).Select(kv => kv.Value);
        var text = string.Concat(orderedTexts);

        return await Task.FromResult(text);
    }

    private static MemoryStream ProcessStream(IFormFile file)
    {
        var stream = new MemoryStream();
        file.CopyTo(stream);
        stream.Position = 0;

        return stream;
    }
}