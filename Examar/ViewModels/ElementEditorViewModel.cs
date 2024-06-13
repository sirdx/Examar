using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Examar.Core;
using Examar.Core.Models;
using Examar.Services;
using Examar.Utils;

namespace Examar.ViewModels;

public partial class ElementEditorViewModel : ViewModel
{
    [ObservableProperty] 
    private ElementMetadata? _elementMetadata;
    
    [ObservableProperty]
    private Element? _targetElement;

    [ObservableProperty] 
    private ObservableCollection<ObservableKeyValuePair<string, string>> _fields = [];

    public ElementEditorViewModel(INavigationService navigationService) : base(navigationService)
    {
        
    }
    
    partial void OnElementMetadataChanging(ElementMetadata? value)
    {
        Fields.Clear();
        
        if (value is null)
        {
            return;
        }
        
        foreach (var key in value.Fields.Keys)
        {
            Fields.Add(new ObservableKeyValuePair<string, string>(key, string.Empty));
        }
    }

    partial void OnTargetElementChanging(Element? value)
    {
        if (value is null)
        {
            return;
        }
        
        foreach (var field in value.Fields)
        {
            Fields.Single(f => f.Key == field.Key).Value = field.Value;
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        NavigationService.Pop();
    }

    [RelayCommand]
    private void Save()
    {
        if (TargetElement is not null)
        {
            SaveExamElement(TargetElement);
            return;
        }

        if (ElementMetadata?.Create() is not Element element)
        {
            return;
        }
        
        SaveExamElement(element);
    }

    private void SaveExamElement(Element element)
    {
        foreach (var field in Fields)
        {
            element.Fields[field.Key] = field.Value;
        }

        var prevViewModel = NavigationService.PopTo<CreatorViewModel>();
        prevViewModel?.AddElement(element);
    }
}
