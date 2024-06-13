using Examar.Core.Models;

namespace Examar.Services;

public interface IExamService
{
    Exam? Read(string filePath);
    void Save(Exam exam, string filePath);
}
