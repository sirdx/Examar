using Examar.Core.Models;

namespace Examar.Core.Services;

public interface IExamThemeService
{
    public IEnumerable<Theme> Themes { get; }

    public bool Add(Theme theme);
    public bool Remove(Theme theme);
    public Theme GetById(string id);
}
