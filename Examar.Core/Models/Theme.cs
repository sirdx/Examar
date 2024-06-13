using QuestPDF.Infrastructure;

namespace Examar.Core.Models;

public abstract class Theme
{
    public string Id { get; private set; }
    public string Name { get; init; }

    protected Theme(string id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public abstract bool Supports(Element element);
    public abstract void Compose(IContainer container, Element element);
}
