using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

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

        document.Add(new Paragraph(quizDetails.Title)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(18)
            .SetMarginBottom(20));

        foreach (var question in quizDetails.Questions)
        {
            var questionContainer = new Div();

            int questionIndex = quizDetails.Questions.Select((q, index) => new { q, index })
                .FirstOrDefault(x => x.q == question)?.index + 1 ?? 0;

            var questionText = new Paragraph($"{questionIndex}. {question.Text}")
                .SetFontSize(14)
                .SetMarginBottom(10);

            questionContainer.Add(questionText);

            foreach (var answer in question.Answers)
            {
                var answerParagraph = new Paragraph()
                    .Add(new Text("O  "))
                    .SetFontSize(12)
                    .SetMarginLeft(10)
                    .Add(new Text(answer.Text)
                        .SetFontSize(12))
                    .SetMultipliedLeading(0.8f);

                questionContainer.Add(answerParagraph);
            }

            questionContainer.SetKeepTogether(true);

            document.Add(questionContainer);
            document.Add(new Paragraph().SetMarginTop(10));
        }

        document.Close();
        return memoryStream.ToArray();
    }
}
