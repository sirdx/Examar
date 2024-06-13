using System.Globalization;
using Examar.Core.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Examar.Elements;

public static class SpaceElement
{
    private const string HeightField = "height";
    
    public static readonly ElementMetadata Metadata = new ElementMetadata(
            "default_space",
            "Elements_Default_Space_Name",
            "Elements_Default_Space_Description",
            "FormatLineSpacing",
            DefaultComposer,
            Previewer)
        .AddField(
            HeightField,
            "Elements_Default_Space_FieldName", 
            "Elements_Default_Space_FieldDescription", 
            ElementMetadata.FieldType.Text);
    
    private static void DefaultComposer(IContainer container, Element element)
    {
        var height = float.Parse(element.Fields[HeightField], CultureInfo.InvariantCulture);
        
        container.Column(column =>
        {
            column.Item().PaddingBottom(height, Unit.Inch);
        });
    }
    
    private static string Previewer(Element element)
    {
        return $"{element.Fields[HeightField]} inch";
    }
}
