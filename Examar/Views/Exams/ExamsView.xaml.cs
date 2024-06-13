using System.Windows;
using Examar.ViewModels;

namespace Examar.Views.Exams;

public partial class ExamsView
{
    public ExamsView()
    {
        InitializeComponent();
        
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        (DataContext as ExamsViewModel)!.RefreshExams();
    }
}
