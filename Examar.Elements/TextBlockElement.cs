using Examar.Core.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Examar.Elements;

public static class TextBlockElement
{
    private const string ContentField = "content";
    
    public static readonly ElementMetadata Metadata = new ElementMetadata(
            "default_textblock",
            "Elements_Default_TextBlock_Name", 
            "Elements_Default_TextBlock_Description", 
            "TextLong",
            DefaultComposer,
            Previewer)
        .AddField(
            ContentField,
            "Elements_Default_TextBlock_FieldName", 
            "Elements_Default_TextBlock_FieldDescription", 
            ElementMetadata.FieldType.MultilineText);

    private static void DefaultComposer(IContainer container, Element element)
    {
        var content = element.Fields[ContentField];
        
        container.Column(column =>
        {
            column.Item().Text(content);
        });
    }
    
    private static string Previewer(Element element)
    {
        return element.Fields[ContentField];
    }
}
