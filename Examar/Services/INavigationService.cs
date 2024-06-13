using Examar.Core;

namespace Examar.Services;

public interface INavigationService
{
    ViewModel CurrentView { get; }
    
    T NavigateTo<T>() where T : ViewModel;
    T Push<T>() where T : ViewModel;
    ViewModel? Pop();
    T? PopTo<T>() where T : ViewModel;
}
