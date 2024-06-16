using System.Text.Json.Serialization;

namespace Examar.Core.Models;

public sealed class Exam
{ 
    private readonly List<Element> _elements = [];
    private readonly List<ElementDto> _elementDtos = [];
    
    public string Title { get; set; }
    
    [JsonIgnore]
    public string? FilePath { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Theme? DefaultTheme { get; set; }

    [JsonIgnore]
    public IEnumerable<Element> Elements => _elements;

    [JsonPropertyName("Elements")]
    public IEnumerable<ElementDto> ElementDtos
    {
        get => _elements.Select(e => e.ToDto());
        set
        {
            _elementDtos.Clear();
            _elementDtos.AddRange(value);
        }
    }
    
    public Exam(string title, Theme? defaultTheme = null)
    {
        Title = title;
        DefaultTheme = defaultTheme;
    }
    
    public void AddElement(Element element)
    {
        if (element.Theme is not null && !element.Theme.Supports(element))
        {
            element.Theme = null;
        }
        
        if (element.Theme is null && DefaultTheme is not null && DefaultTheme.Supports(element))
        {
            element.Theme = DefaultTheme;
        }
        
        _elements.Add(element);
    }

    public void RemoveElement(Element element)
    {
        _elements.Remove(element);
    }

    public void ClearElements()
    {
        _elements.Clear();
    }
    
    // TODO: Perhaps could be implemented better in the future
    public void LoadElementsFromDtos(Func<ElementDto, Element> factory)
    {
        ClearElements();
        
        foreach (var dto in _elementDtos)
        {
            AddElement(factory(dto));
        }
    }
}
