using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Examar.Core;
using Examar.Core.Models;
using Examar.Services;

namespace Examar.ViewModels;

public partial class MainViewModel : ViewModel
{
    private readonly IConfigurationService _configurationService;
    private readonly IExamService _examService;

    [ObservableProperty] 
    private string _examarVersion = "Examar PREVIEW";

    [ObservableProperty] 
    private ObservableCollection<Exam> _pinnedExams = [];

    public MainViewModel(INavigationService navigationService,
        IConfigurationService configurationService,
        IExamService examService) : base(navigationService)
    {
        _configurationService = configurationService;
        _examService = examService;

        WeakReferenceMessenger.Default.Register<PinExamMessage>(this, (_, msg) =>
        {
            var exam = msg.Value;
            PinnedExams.Add(exam);
        });
        
        WeakReferenceMessenger.Default.Register<UnpinExamMessage>(this, (_, msg) =>
        {
            var exam = msg.Value;
            PinnedExams.Remove(PinnedExams.Single(e => e.FilePath == exam.FilePath));
        });
        
        NavigateHomeView();
    }

    public void RefreshPinnedExams()
    {
        PinnedExams.Clear();
        
        foreach (var filePath in _configurationService.Configuration.PinnedExams)
        {
            if (!File.Exists(filePath))
            {
                continue;
            }

            var exam = _examService.Read(filePath);
            PinnedExams.Add(exam);
        }
    }

    [RelayCommand]
    private void NavigateHomeView()
    {
        NavigationService.NavigateTo<HomeViewModel>();
    }

    [RelayCommand]
    private void NavigateExamsView()
    {
        NavigationService.NavigateTo<ExamsViewModel>();
    }

    [RelayCommand]
    private void NavigateSettingsView()
    {
        NavigationService.NavigateTo<SettingsViewModel>();
    }

    [RelayCommand]
    private void NavigateCreateExam()
    {
        NavigationService.Push<CreatorViewModel>();
    }

    [RelayCommand]
    private void EditPinnedExam(Exam exam)
    {
        var viewModel = NavigationService.Push<CreatorViewModel>();
        viewModel.FillExam(exam);
    }
}
