using System.Windows;
using System.Windows.Controls;
using Examar.ViewModels;

namespace Examar.Views.ElementSelector;

public partial class ElementSelectorView
{
    public ElementSelectorView()
    {
        InitializeComponent();

        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        (DataContext as ElementSelectorViewModel)!.RefreshElements();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count == 0)
        {
            return;
        }
        
        (DataContext as ElementSelectorViewModel)!.SelectElement();
    }
}
