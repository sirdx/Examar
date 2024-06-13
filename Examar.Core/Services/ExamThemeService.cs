using Examar.Core.Models;

namespace Examar.Core.Services;

public class ExamThemeService : IExamThemeService
{
    private readonly HashSet<Theme> _themes = [];

    public IEnumerable<Theme> Themes => _themes;
    
    public bool Add(Theme theme) => _themes.Add(theme);

    public bool Remove(Theme theme) => _themes.Remove(theme);

    public Theme GetById(string id) => _themes.Single(t => t.Id == id);
}
