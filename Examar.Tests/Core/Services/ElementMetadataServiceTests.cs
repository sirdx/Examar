using Examar.Core.Models;
using Examar.Core.Services;

namespace Examar.Tests.Core.Services;

public class ElementMetadataServiceTests
{
    private readonly ElementMetadata _testElementMetadata = new ElementMetadata(
        "test",
        "Element",
        "Test element",
        "None",
        null!,
        _ => "Preview");
        
    private ElementMetadataService _elementMetadataService;
    
    [SetUp]
    public void Setup()
    {
        _elementMetadataService = new ElementMetadataService();
    }

    [Test]
    public void Add_AddMetadata()
    {
        _elementMetadataService.Add(_testElementMetadata);
        Assert.That(_elementMetadataService.Metadata.Last(), Is.EqualTo(_testElementMetadata), "Metadata should be added");
    }

    [Test]
    public void GetById_ReturnMetadata()
    {
        var metadata = _elementMetadataService.GetById(_testElementMetadata.Id);
        Assert.That(metadata,  Is.EqualTo(_testElementMetadata), "Metadata should be returned by ID");
    }

    [Test]
    public void Remove_RemoveMetadata()
    {
        _elementMetadataService.Remove(_testElementMetadata);
        Assert.That(_elementMetadataService.Metadata.Count(), Is.Zero, "Metadata should be removed");
    }
}