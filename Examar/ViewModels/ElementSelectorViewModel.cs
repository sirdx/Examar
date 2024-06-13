using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Examar.Core;
using Examar.Core.Models;
using Examar.Core.Services;
using Examar.Services;

namespace Examar.ViewModels;

public partial class ElementSelectorViewModel : ViewModel
{
    private readonly IElementMetadataService _elementMetadataService;

    [ObservableProperty] 
    private ObservableCollection<ElementMetadata> _elements = [];

    [ObservableProperty] 
    private ElementMetadata? _selectedElement;

    public ElementSelectorViewModel(INavigationService navigationService, IElementMetadataService elementMetadataService) 
        : base(navigationService)
    {
        _elementMetadataService = elementMetadataService;
    }
    
    public void RefreshElements()
    {
        Elements.Clear();

        foreach (var metadata in _elementMetadataService.Metadata)
        {
            Elements.Add(metadata);
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        NavigationService.Pop();
    }

    [RelayCommand]
    public void SelectElement()
    {
        var viewModel = NavigationService.Push<ElementEditorViewModel>();
        viewModel.ElementMetadata = SelectedElement;
        SelectedElement = null;
    }
}
