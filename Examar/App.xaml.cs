using System.Windows;
using Examar.Core;
using Examar.Core.Services;
using Examar.Elements;
using Examar.Services;
using Examar.ViewModels;
using Examar.Views;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace Examar;

public partial class App
{
    private readonly IServiceProvider _serviceProvider;
    
    public App()
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var services = new ServiceCollection();

        services.AddSingleton<MainWindow>(provider => new MainWindow
        {
            DataContext = provider.GetRequiredService<MainViewModel>()
        });
        services.AddSingleton<MainViewModel>();

        services.AddSingleton<IConfigurationService, ConfigurationService>();
        services.AddSingleton<IElementMetadataService, ElementMetadataService>();
        services.AddSingleton<IExamThemeService, ExamThemeService>();
        services.AddSingleton<IElementDtoService, ElementDtoService>();
        services.AddSingleton<IExamService, ExamService>();

        services.AddTransient<HomeViewModel>();
        services.AddTransient<ExamsViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<CreatorViewModel>();
        services.AddTransient<ElementSelectorViewModel>();
        services.AddTransient<ElementEditorViewModel>();

        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<Func<Type, ViewModel>>(provider =>
            viewModelType => (ViewModel)provider.GetRequiredService(viewModelType));

        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var elementMetadataService = _serviceProvider.GetRequiredService<IElementMetadataService>();
        elementMetadataService.Add(InstructionElement.Metadata);
        elementMetadataService.Add(TextBlockElement.Metadata);
        elementMetadataService.Add(SpaceElement.Metadata);
        elementMetadataService.Add(LatexElement.Metadata);
        elementMetadataService.Add(WritingAreaElement.Metadata);
        
        var window = _serviceProvider.GetRequiredService<MainWindow>();
        window.Show();
        
        base.OnStartup(e);
    }
}
