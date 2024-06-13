using CommunityToolkit.Mvvm.ComponentModel;
using Examar.Services;

namespace Examar.Core;

public abstract partial class ViewModel : ObservableObject
{
    [ObservableProperty]
    private INavigationService _navigationService;

    protected ViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
    }
}
