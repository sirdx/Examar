using System.Windows;
using Examar.ViewModels;

namespace Examar.Views;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        (DataContext as MainViewModel)!.RefreshPinnedExams();
    }
}
