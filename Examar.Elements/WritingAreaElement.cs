using Examar.Core.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Examar.Elements;

public static class WritingAreaElement
{
    private const string LinesField = "lines";
    
    public static readonly ElementMetadata Metadata = new ElementMetadata(
            "default_writingarea",
            "Elements_Default_WritingArea_Name", 
            "Elements_Default_WritingArea_Description", 
            "FormTextArea",
            DefaultComposer, 
            Previewer)
        .AddField(
            LinesField,
            "Elements_Default_WritingArea_FieldName", 
            "Elements_Default_WritingArea_FieldDescription", 
            ElementMetadata.FieldType.Text);
    
    private static void DefaultComposer(IContainer container, Element element)
    {
        var lines = uint.Parse(element.Fields[LinesField]);
        
        container.Text(text =>
        {
            text.ClampLines((int)lines, string.Empty);
            text.DefaultTextStyle(style => style.LineHeight(2F));
            
            for (uint i = 0; i < lines; i++)
            {
                text.Span(new string('.', 200));
            }
        });
    }
    
    private static string Previewer(Element element)
    {
        return $"{element.Fields[LinesField]} Lines";
    }
}
