using Examar.Core.Models;

namespace Examar.Core.Services;

public interface IElementMetadataService
{
    public IEnumerable<ElementMetadata> Metadata { get; }

    public bool Add(ElementMetadata metadata);
    public bool Remove(ElementMetadata metadata);
    public ElementMetadata GetById(string id);
}
