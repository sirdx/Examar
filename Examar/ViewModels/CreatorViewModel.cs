using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Examar.Core;
using Examar.Core.Models;
using Examar.Export;
using Examar.Services;
using QuestPDF.Fluent;

namespace Examar.ViewModels;

public partial class CreatorViewModel : ViewModel
{
    private readonly IExamService _examService;

    private bool _existingExam;
    
    [ObservableProperty] 
    private string _title = string.Empty;
    
    [ObservableProperty] 
    private string _filePath = string.Empty;
    
    [ObservableProperty] 
    private ObservableCollection<Element> _elements = [];
    
    [ObservableProperty]
    private Element? _selectedElement;

    public CreatorViewModel(INavigationService navigationService,
        IExamService examService) : base(navigationService)
    {
        _examService = examService;
    }

    public void FillExam(Exam exam)
    {
        Title = exam.Title;
        FilePath = exam.FilePath ?? string.Empty;

        foreach (var element in exam.Elements)
        {
            Elements.Add(element);
        }

        _existingExam = true;
    }

    public void AddElement(Element element)
    {
        var elementIndex = Elements.IndexOf(element);
            
        if (elementIndex == -1)
        {
            Elements.Add(element);
            return;
        }
        
        // Fix element's preview not updating
        Elements.Remove(element);
        Elements.Insert(elementIndex, element);
    }
    
    [RelayCommand]
    private void SelectFilePath()
    {
        // Configure save file dialog box
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            FileName = Title, // Default file name
            DefaultExt = ".json", // Default file extension
            Filter = "Examar JSON (.json)|*.json" // Filter files by extension
        };

        // Show save file dialog box
        bool? result = dialog.ShowDialog();

        // Process save file dialog box results
        if (result == true)
        {
            // Save document
            FilePath = dialog.FileName;
        }
    }

    [RelayCommand]
    private void EditElement(Element element)
    {
        var viewModel = NavigationService.Push<ElementEditorViewModel>();
        viewModel.ElementMetadata = element.Metadata;
        viewModel.TargetElement = element;
    }

    [RelayCommand]
    private void DuplicateElement(Element element)
    {
        Elements.Add((Element)element.Clone());
    }

    [RelayCommand]
    private void RemoveElement(Element element)
    {
        Elements.Remove(element);
    }

    private Exam CreateExam()
    {
        var exam = new Exam(Title);

        foreach (var element in Elements)
        {
            exam.AddElement(element);
        }

        return exam;
    }

    [RelayCommand]
    private void SaveExam()
    {
        var exam = CreateExam();

        if (_existingExam)
        {
            exam.FilePath = FilePath;
        }
        
        _examService.Save(exam, FilePath);
        
        NavigationService.Pop();
    }

    [RelayCommand]
    private void ExportExam()
    {
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            FileName = Title,
            DefaultExt = ".pdf",
            Filter = "PDF file (.pdf)|*.pdf"
        };
        
        if (dialog.ShowDialog() == false)
        {
            return;
        }
        
        var pdfPath = dialog.FileName;
        var exam = CreateExam();
        var document = new ExamDocument(exam);
        document.GeneratePdf(pdfPath);
        
        var msgBoxResult = MessageBox.Show("Exam PDF has been exported successfully.\nDo you want to open it?", "Examar", MessageBoxButton.YesNo, MessageBoxImage.Information);
        
        if (msgBoxResult is MessageBoxResult.Yes)
        {
            Process.Start("explorer", $"\"{pdfPath}\"");
        }
    }

    [RelayCommand]
    private void CancelExam()
    {
        NavigationService.Pop();
    }

    [RelayCommand]
    private void NavigateElementSelector()
    {
        NavigationService.Push<ElementSelectorViewModel>();
    }
}
