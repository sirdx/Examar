using Examar.Core.Models;

namespace Examar.Core.Services;

public sealed class ElementMetadataService : IElementMetadataService
{
    private readonly HashSet<ElementMetadata> _metadata = [];

    public IEnumerable<ElementMetadata> Metadata => _metadata;
    
    public bool Add(ElementMetadata metadata) => _metadata.Add(metadata);

    public bool Remove(ElementMetadata metadata) =>_metadata.Remove(metadata);

    public ElementMetadata GetById(string id) => _metadata.Single(m => m.Id == id);
}
