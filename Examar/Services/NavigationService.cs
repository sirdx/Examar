using CommunityToolkit.Mvvm.ComponentModel;
using Examar.Core;

namespace Examar.Services;

public class NavigationService : ObservableObject, INavigationService
{
    private readonly Func<Type, ViewModel> _viewModelFactory;
    private readonly Stack<ViewModel> _viewModels = [];

    public ViewModel CurrentView => _viewModels.Peek();

    public NavigationService(Func<Type, ViewModel> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public T NavigateTo<T>() where T : ViewModel
    {
        if (_viewModels.TryPeek(out var result) && result is T target)
        {
            return target;
        }
        
        _viewModels.Clear();
        return Push<T>();
    }

    public T Push<T>() where T : ViewModel
    {
        var viewModel = _viewModelFactory.Invoke(typeof(T));
        
        if (_viewModels.TryPeek(out var result) && result is T)
        {
            _viewModels.Pop();
        }
        
        _viewModels.Push(viewModel);
        OnPropertyChanged(nameof(CurrentView));
        return (T)viewModel;
    }

    public ViewModel? Pop()
    {
        _viewModels.Pop();
        OnPropertyChanged(nameof(CurrentView));
        _viewModels.TryPeek(out var prevViewModel);
        return prevViewModel;
    }

    public T? PopTo<T>() where T : ViewModel
    {
        var viewModelPopped = false;
        
        while (_viewModels.TryPeek(out var viewModel))
        {
            if (viewModel is T result)
            {
                if (viewModelPopped)
                {
                    OnPropertyChanged(nameof(CurrentView));
                }

                return result;
            }

            _viewModels.Pop();
            viewModelPopped = true;
        }

        if (viewModelPopped)
        {
            OnPropertyChanged(nameof(CurrentView));
        }
        
        return null;
    }
}
