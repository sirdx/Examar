namespace Examar.Core.Models;

public static class ElementDtoExtensions
{
    public static ElementDto ToDto(this Element element)
    {
        return new ElementDto
        {
            TypeId = element.Metadata.Id,
            Id = element.Id,
            Fields = element.Fields,
            ThemeId = element.Theme?.Id
        };
    }
}
