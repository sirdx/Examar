using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Examar.Core;
using Examar.Core.Models;
using Examar.Services;

namespace Examar.ViewModels;

public class PinExamMessage(Exam value) : ValueChangedMessage<Exam>(value);
public class UnpinExamMessage(Exam value) : ValueChangedMessage<Exam>(value);

public partial class ExamsViewModel : ViewModel
{
    private readonly IConfigurationService _configurationService;
    private readonly IExamService _examService;

    [ObservableProperty] 
    private ObservableCollection<Exam> _exams = [];

    [ObservableProperty] 
    private Exam? _selectedExam;

    public ExamsViewModel(INavigationService navigationService, 
        IConfigurationService configurationService,
        IExamService examService) : base(navigationService)
    {
        _configurationService = configurationService;
        _examService = examService;
    }

    [RelayCommand]
    public void RefreshExams()
    {
        Exams.Clear();

        foreach (var filePath in _configurationService.Configuration.Exams)
        {
            var exam = _examService.Read(filePath);

            if (exam is null)
            {
                continue;
            }
            
            Exams.Add(exam);
        }
    }

    [RelayCommand]
    private void PinExam(Exam exam)
    {
        _configurationService.Configuration.PinnedExams.Add(exam.FilePath);
        _configurationService.Save();
        WeakReferenceMessenger.Default.Send(new PinExamMessage(exam));
    }

    [RelayCommand]
    private void UnpinExam(Exam exam)
    {
        _configurationService.Configuration.PinnedExams.Remove(exam.FilePath);
        _configurationService.Save();
        WeakReferenceMessenger.Default.Send(new UnpinExamMessage(exam));
    }

    [RelayCommand]
    private void EditExam(Exam exam)
    {
        var viewModel = NavigationService.Push<CreatorViewModel>();
        viewModel.FillExam(exam);
    }

    [RelayCommand]
    private void DeleteExam(Exam exam)
    {
        if (File.Exists(exam.FilePath))
        {
            File.Delete(exam.FilePath);
        }
        else
        {
            MessageBox.Show("File does not exist! Removing item from the list...", "Warning", MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
        
        RemoveExamFromList(exam);
    }

    [RelayCommand]
    private void RemoveExamFromList(Exam exam)
    {
        _configurationService.Configuration.Exams.Remove(exam.FilePath);
        _configurationService.Save();
        Exams.Remove(exam);
    }
}
