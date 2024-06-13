using System.IO;
using System.Text.Json;
using Examar.Core.Models;
using Examar.Core.Services;

namespace Examar.Services;

public class ExamService : IExamService
{
    private readonly IElementDtoService _elementDtoService;
    private readonly IConfigurationService _configurationService;

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true
    };

    public ExamService(IElementDtoService elementDtoService, IConfigurationService configurationService)
    {
        _elementDtoService = elementDtoService;
        _configurationService = configurationService;
    }
    
    public Exam? Read(string filePath)
    {
        var jsonStr = File.ReadAllText(filePath);
        var exam = JsonSerializer.Deserialize<Exam>(jsonStr);

        if (exam is null)
        {
            return null;
        }
            
        exam.LoadElementsFromDtos(dto => _elementDtoService.ParseElement(dto));
        exam.FilePath = filePath;

        return exam;
    }
    
    public void Save(Exam exam, string filePath)
    {
        var jsonStr = JsonSerializer.Serialize(exam, _serializerOptions);
        File.WriteAllText(filePath, jsonStr);

        // If saved to a new location
        if (!(exam.FilePath is not null && exam.FilePath == filePath))
        {
            AddToExams(filePath);
        }
        
        exam.FilePath = filePath;
    }

    private void AddToExams(string filePath)
    {
        _configurationService.Configuration.Exams.Add(filePath);
        _configurationService.Save();
    }
}
