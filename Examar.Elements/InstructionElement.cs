using Examar.Core.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Examar.Elements;

public static class InstructionElement
{
    private const string ContentField = "content";
    
    public static readonly ElementMetadata Metadata = new ElementMetadata(
            "default_instruction",
            "Elements_Default_Instruction_Name",
            "Elements_Default_Instruction_Description",
            "ScriptText",
            DefaultComposer,
            Previewer)
        .AddField(
            ContentField,
            "Elements_Default_Instruction_FieldName", 
            "Elements_Default_Instruction_FieldDescription", 
            ElementMetadata.FieldType.Text);
    
    private static void DefaultComposer(IContainer container, Element element)
    {
        var content = element.Fields[ContentField];
        
        container.Column(column =>
        {
            column.Item().Text(content).Bold();
        });
    }
    
    private static string Previewer(Element element)
    {
        return element.Fields[ContentField];
    }
}
