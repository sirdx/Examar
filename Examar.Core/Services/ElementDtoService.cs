using Examar.Core.Models;

namespace Examar.Core.Services;

public sealed class ElementDtoService : IElementDtoService
{
    private readonly IElementMetadataService _elementMetadataService;
    private readonly IExamThemeService _examThemeService;

    public ElementDtoService(IElementMetadataService elementMetadataService,
        IExamThemeService examThemeService)
    {
        _elementMetadataService = elementMetadataService;
        _examThemeService = examThemeService;
    }
    
    public Element ParseElement(ElementDto elementDto)
    {
        var metadata = _elementMetadataService.GetById(elementDto.TypeId);
        var theme = elementDto.ThemeId is not null ? _examThemeService.GetById(elementDto.ThemeId) : null;   

        var element = Element.CreateFromExisting(metadata, elementDto.Id);
        element.Theme = theme;

        foreach (var (key, _) in element.Fields)
        {
            element.Fields[key] = elementDto.Fields[key];
        }
        
        return element;
    }
}
