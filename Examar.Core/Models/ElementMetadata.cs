using System.Text.Json.Serialization;
using QuestPDF.Infrastructure;

namespace Examar.Core.Models;

public sealed class ElementMetadata
{
    public enum FieldType
    {
        Text,
        MultilineText
    }
    
    public record struct Field(string Name, string Description, FieldType Type);
    
    public string Id { get; }
    
    [JsonIgnore]
    public string Name { get; }
    
    [JsonIgnore]
    public string Description { get; }
    
    [JsonIgnore]
    public string Icon { get; }
    
    [JsonIgnore]
    public Dictionary<string, Field> Fields { get; } = [];

    [JsonIgnore]
    public Action<IContainer, Element> DefaultComposer { get; }
    
    [JsonIgnore]
    public Func<Element, string> Previewer { get; }

    public ElementMetadata(string id, string name, string description, string icon, Action<IContainer, Element> defaultComposer, Func<Element, string> previewer)
    {
        Id = id;
        Name = name;
        Description = description;
        Icon = icon;
        DefaultComposer = defaultComposer;
        Previewer = previewer;
    }

    public ElementMetadata AddField(string key, string name, string description, FieldType type)
    {
        Fields[key] = new Field(name, description, type);
        return this;
    }

    public Element Create()
    {
        return new Element(this);
    }
}
