using QuestPDF.Infrastructure;

namespace Examar.Core.Models;

public sealed class Element : IComponent, ICloneable
{
    public ElementMetadata Metadata { get; }
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Dictionary<string, string> Fields { get; private set; }
    public Theme? Theme { get; set; }
    
    public string Preview => Metadata.Previewer(this);
    
    public Element(ElementMetadata metadata)
    {
        Metadata = metadata;
        Fields = Metadata.Fields.Select(x => new KeyValuePair<string, string>(x.Key, string.Empty)).ToDictionary();
    }
    
    public void Compose(IContainer container)
    {
        if (Theme is null)
        {
            Metadata.DefaultComposer(container, this);
            return;
        }

        Theme.Compose(container, this);
    }
    
    public object Clone()
    {
        var clone = (Element)MemberwiseClone();
        clone.Id = Guid.NewGuid();
        clone.Fields = new Dictionary<string, string>(clone.Fields);
        return clone;
    }

    public static Element CreateFromExisting(ElementMetadata metadata, Guid id)
    {
        return new Element(metadata)
        {
            Id = id
        };
    }
}
