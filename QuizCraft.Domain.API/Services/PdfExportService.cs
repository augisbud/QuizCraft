using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using QuizCraft.Domain.API.Repositories;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Services;

public class PdfExportService(IQuizService quizService) : IPdfExportService
{
    public async Task<byte[]> GenerateQuizPdfAsync(Guid quizId, string token)
    {
        var quizDetails = await quizService.RetrieveQuestions(quizId, token);

        using var memoryStream = new MemoryStream();
        using var writer = new PdfWriter(memoryStream);
        using var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        //title
        document.Add(new Paragraph(quizDetails.Title)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(18));

        //table
        var table = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
        table.AddHeaderCell("Question");
        table.AddHeaderCell("Option 1");
        table.AddHeaderCell("Option 2");
        table.AddHeaderCell("Option 3");
        table.AddHeaderCell("Option 4");

        //questions
        foreach (var question in quizDetails.Questions)
        {
            table.AddCell(question.Text);
            for (int i = 0; i < 4; i++)
            {
                table.AddCell(i < question.Answers.Count ? question.Answers[i].Text : ""); // Fewer options fallback
            }
        }

        document.Add(table);
        document.Close();

        return memoryStream.ToArray();
    }
}
