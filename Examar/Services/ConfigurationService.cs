using System.IO;
using System.Text.Json;
using Examar.Utils;

namespace Examar.Services;

public sealed class ConfigurationService : IConfigurationService
{
    private const string Directory = "Examar";
    private const string FileName = "config.json";
    
    private readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private readonly string DirectoryPath;
    private readonly string Path;
    private readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    public Configuration Configuration { get; private set; } = new();

    public ConfigurationService()
    {
        DirectoryPath = System.IO.Path.Combine(AppDataPath, Directory);
        Path = System.IO.Path.Combine(DirectoryPath, FileName);
        
        Reload();
    }
    
    public void Reload()
    {
        if (!File.Exists(Path))
        {
            Save();
            return;
        }

        try
        {
            var jsonStr = File.ReadAllText(Path);
            Configuration = JsonSerializer.Deserialize<Configuration>(jsonStr) ?? throw new NullReferenceException();
        }
        catch (Exception ex)
        {
            
        }
    }

    public void Save()
    {
        try
        {
            var jsonStr = JsonSerializer.Serialize(Configuration, SerializerOptions);
            
            System.IO.Directory.CreateDirectory(DirectoryPath);
            File.WriteAllText(Path, jsonStr);
        }
        catch (Exception ex)
        {
            
        }
    }
}
