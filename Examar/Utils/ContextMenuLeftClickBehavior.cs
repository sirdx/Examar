using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Examar.Utils;

public static class ContextMenuLeftClickBehavior
{
    public static bool GetIsLeftClickEnabled(DependencyObject obj)
    {
        return (bool)obj.GetValue(IsLeftClickEnabledProperty);
    }

    public static void SetIsLeftClickEnabled(DependencyObject obj, bool value)
    {
        obj.SetValue(IsLeftClickEnabledProperty, value);
    }

    public static readonly DependencyProperty IsLeftClickEnabledProperty = DependencyProperty.RegisterAttached(
        "IsLeftClickEnabled", 
        typeof(bool), 
        typeof(ContextMenuLeftClickBehavior), 
        new UIPropertyMetadata(false, OnIsLeftClickEnabledChanged));

    private static void OnIsLeftClickEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is not UIElement uiElement)
        {
            return;
        }
        
        var isEnabled = e.NewValue is bool value && value;

        if (isEnabled)
        {
            if (uiElement is ButtonBase @base)
            {
                @base.Click += OnMouseLeftButtonUp;
            }
            else
            {
                uiElement.MouseLeftButtonUp += OnMouseLeftButtonUp;
            }
        }
        else
        {
            if (uiElement is ButtonBase @base)
            {
                @base.Click -= OnMouseLeftButtonUp;
            }
            else
            {
                uiElement.MouseLeftButtonUp -= OnMouseLeftButtonUp;
            }
        }
    }

    private static void OnMouseLeftButtonUp(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement fe)
        {
            return;
        }
        
        fe.ContextMenu.PlacementTarget = fe;
        
        // if we use binding in our context menu, then it's DataContext won't be set when we show the menu on left click
        // (it seems setting DataContext for ContextMenu is hardcoded in WPF when user right clicks on a control, although I'm not sure)
        // so we have to set up ContextMenu.DataContext manually here
        if (fe.ContextMenu is { DataContext: null })
        {
            fe.ContextMenu.SetBinding(FrameworkElement.DataContextProperty, new Binding { Source = fe.DataContext });
        }

        fe.ContextMenu.IsOpen = true;
    }
}
