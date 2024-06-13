using CSharpMath.SkiaSharp;
using Examar.Core.Models;
using Examar.Elements.Utils;
using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Examar.Elements;

public static class LatexElement
{
    private class LatexComponent : IDynamicComponent
    {
        private readonly string _code;

        public LatexComponent(string code)
        {
            _code = code;
        }
    
        public DynamicComponentComposeResult Compose(DynamicContext context)
        {
            var latexPainter = new MathPainter
            {
                LaTeX = _code
            };
        
            var content = context.CreateElement(container =>
            {
                var width = context.AvailableSize.Width;
                var latexPainterSize = latexPainter.Measure(width);
            
                container.Height(latexPainterSize.Height).SkiaSharpCanvas((canvas, _) =>
                {
                    latexPainter.Draw(canvas);
                });
            });
        
            return new DynamicComponentComposeResult
            {
                Content = content,
                HasMoreContent = false
            };
        }
    }
    
    private const string CodeField = "code";
    
    public static readonly ElementMetadata Metadata = new ElementMetadata(
            "default_latex",
            "Elements_Default_LaTeX_Name",
            "Elements_Default_LaTeX_Description",
            "Function",
            DefaultComposer, 
            Previewer)
        .AddField(
            CodeField,
            "Elements_Default_LaTeX_FieldName",
            "Elements_Default_LaTeX_FieldDescription",
            ElementMetadata.FieldType.MultilineText);

    private static void DefaultComposer(IContainer container, Element element)
    {
        var code = element.Fields[CodeField];
        
        container.Column(column =>
        {
            column.Item().Dynamic(new LatexComponent(code));
        });
    }
    
    private static string Previewer(Element element)
    {
        return element.Fields[CodeField];
    }
}
