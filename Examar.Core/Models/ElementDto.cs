using System.Text.Json.Serialization;

namespace Examar.Core.Models;

public sealed class ElementDto
{
    public required string TypeId { get; init; }
    public required Guid Id { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ThemeId { get; set; }
    public required Dictionary<string, string> Fields { get; init; }
}
