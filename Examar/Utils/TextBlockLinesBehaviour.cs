using System.Windows;
using System.Windows.Controls;

namespace Examar.Utils;

public static class TextBlockLinesBehaviour
{
    public static readonly DependencyProperty MaxLinesProperty =
        DependencyProperty.RegisterAttached(
            "MaxLines",
            typeof(int),
            typeof(TextBlockLinesBehaviour),
            new PropertyMetadata(default(int), OnMaxLinesPropertyChangedCallback));

    public static void SetMaxLines(DependencyObject element, int value)
    {
        element.SetValue(MaxLinesProperty, value);
    }

    public static int GetMaxLines(DependencyObject element)
    {
        return (int)element.GetValue(MaxLinesProperty);
    }

    private static void OnMaxLinesPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TextBlock element)
        {
            return;
        }
        
        element.MaxHeight = GetLineHeight(element) * GetMaxLines(element);
    }

    public static readonly DependencyProperty MinLinesProperty =
        DependencyProperty.RegisterAttached(
            "MinLines",
            typeof(int),
            typeof(TextBlockLinesBehaviour),
            new PropertyMetadata(default(int), OnMinLinesPropertyChangedCallback));

    public static void SetMinLines(DependencyObject element, int value)
    {
        element.SetValue(MinLinesProperty, value);
    }

    public static int GetMinLines(DependencyObject element)
    {
        return (int)element.GetValue(MinLinesProperty);
    }

    private static void OnMinLinesPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TextBlock element)
        {
            return;
        }
        
        element.MinHeight = GetLineHeight(element) * GetMinLines(element);
    }

    private static double GetLineHeight(TextBlock textBlock)
    {
        var lineHeight = textBlock.LineHeight;
        if (double.IsNaN(lineHeight))
        {
            lineHeight = Math.Ceiling(textBlock.FontSize * textBlock.FontFamily.LineSpacing);
        }
        
        return lineHeight;
    }
}
