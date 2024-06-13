using Examar.Core.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Examar.Export;

public sealed class ExamDocument : IDocument
{
    public Exam Exam { get; }

    public ExamDocument(Exam exam)
    {
        Exam = exam;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;
    
    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(1F, Unit.Inch);
            
            page.Content().Column(column =>
            {
                foreach (var examElement in Exam.Elements)
                {
                    column.Item()
                        .Component(examElement);
                }
            });
            
            page.Footer().AlignCenter().Text(x =>
            {
                x.CurrentPageNumber();
                x.Span(" / ");
                x.TotalPages();
            });
        });
    }
}
