namespace Examar.Utils;

public sealed class Configuration
{
    public List<string> Exams { get; init; } = [];
    public List<string> PinnedExams { get; init; } = [];
}
