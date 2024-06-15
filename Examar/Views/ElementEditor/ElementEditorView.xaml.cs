using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Examar.Core.Models;
using Examar.ViewModels;

namespace Examar.Views.ElementEditor;

public partial class ElementEditorView
{
    public ElementEditorView()
    {
        InitializeComponent();
        
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var viewModel = (DataContext as ElementEditorViewModel)!;
        var i = 0;

        if (viewModel.ElementMetadata is null)
        {
            return;
        }
        
        foreach (var (_, field) in viewModel.ElementMetadata.Fields)
        {
            var binding = new Binding($"Fields[{i}].Value")
            {
                Source = viewModel,
                Mode = BindingMode.TwoWay
            };
                
            var uiElement = GenerateUiElement(binding, field);
            CustomizationPanel.Children.Add(uiElement);
            i++;
        }
    }

    private UIElement GenerateUiElement(Binding binding, ElementMetadata.Field field)
    {
        var stackPanel = new StackPanel
        {
            Margin = new Thickness(0.0, 0.0, 0.0, 20.0)
        };
        
        var nameTextBlock = new TextBlock
        {
            Text = Examar.Resources.ResourceManager.GetString(field.Name),
            FontSize = 12.0,
            FontWeight = FontWeights.Medium,
            Foreground = new SolidColorBrush(Color.FromArgb(0xAF, 0xFF, 0xFF, 0xFF)),
            Margin = new Thickness(0.0, 0.0, 0.0, 5.0)
        };
        var descriptionTextBlock = new TextBlock
        {
            Text = Examar.Resources.ResourceManager.GetString(field.Description),
            FontSize = 10.0,
            Foreground = new SolidColorBrush(Color.FromArgb(0x6F, 0xFF, 0xFF, 0xFF)),
            Margin = new Thickness(0.0, 0.0, 0.0, 15.0)
        };
        stackPanel.Children.Add(nameTextBlock);
        stackPanel.Children.Add(descriptionTextBlock);

        switch (field.Type)
        {
            case ElementMetadata.FieldType.Text:
            {
                var textBox = new TextBox
                {
                    Style = FindResource("PlainTextBox") as Style
                };
                textBox.SetBinding(TextBox.TextProperty, binding);
                stackPanel.Children.Add(textBox);
                break;
            }
            case ElementMetadata.FieldType.MultilineText:
            {
                var textBox = new TextBox
                {
                    AcceptsReturn = true
                };
                textBox.SetBinding(TextBox.TextProperty, binding);
                stackPanel.Children.Add(textBox);
                break;
            }
        }

        return stackPanel;
    }
}
