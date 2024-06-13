using Examar.Utils;

namespace Examar.Services;

public interface IConfigurationService
{
    Configuration Configuration { get; }
    
    void Reload();
    void Save();
}
