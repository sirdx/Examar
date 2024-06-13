using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Examar.Utils;

public class DataGridVisualHelper
{
    public static readonly DependencyProperty EnableRowsMoveProperty =
        DependencyProperty.RegisterAttached("EnableRowsMove", typeof(bool), typeof(DataGridVisualHelper), new PropertyMetadata(false, EnableRowsMoveChanged));
    
    private static readonly DependencyProperty DraggedItemProperty =
        DependencyProperty.RegisterAttached("DraggedItem", typeof(object), typeof(DataGridVisualHelper), new PropertyMetadata(null));

    public static bool GetEnableRowsMove(DataGrid obj)
    {
        return (bool)obj.GetValue(EnableRowsMoveProperty);
    }

    public static void SetEnableRowsMove(DataGrid obj, bool value)
    {
        obj.SetValue(EnableRowsMoveProperty, value);
    }

    private static object GetDraggedItem(DependencyObject obj)
    {
        return obj.GetValue(DraggedItemProperty);
    }

    private static void SetDraggedItem(DependencyObject obj, object? value)
    {
        obj.SetValue(DraggedItemProperty, value);
    }

    private static void EnableRowsMoveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not DataGrid grid)
        {
            return;
        }
        
        if ((bool)e.NewValue)
        {
            grid.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            grid.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            grid.PreviewMouseMove += OnMouseMove;
            return;
        }

        grid.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
        grid.PreviewMouseLeftButtonUp -= OnMouseLeftButtonUp;
        grid.PreviewMouseMove -= OnMouseMove;
    }

    private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not DataGrid dataGrid)
        {
            return;
        }
        
        var row = TryFindFromPoint<DataGridRow>(dataGrid, e.GetPosition(dataGrid));
        if (row is null || row.IsEditing)
        {
            return;
        }
        
        SetDraggedItem(dataGrid, row.Item);
    }

    private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (sender is not DataGrid dataGrid)
        {
            return;
        }
        
        var draggedItem = GetDraggedItem(dataGrid);
        if (draggedItem is null)
        {
            return;
        }
        
        ExchangeItems(sender, dataGrid.SelectedItem);
        dataGrid.SelectedItem = draggedItem;

        SetDraggedItem(dataGrid, null);
    }

    private static void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (sender is not DataGrid dataGrid)
        {
            return;
        }
        
        var draggedItem = GetDraggedItem(dataGrid);
        if (draggedItem is null)
        {
            return;
        }
        
        var row = TryFindFromPoint<DataGridRow>(dataGrid, e.GetPosition(dataGrid));
        if (row is null || row.IsEditing)
        {
            return;
        }
        
        ExchangeItems(sender, row.Item);
    }

    private static void ExchangeItems(object sender, object targetItem)
    {
        if (sender is not DataGrid dataGrid)
        {
            return;
        }
        
        var draggedItem = GetDraggedItem(dataGrid);
        if (draggedItem is null)
        {
            return;
        }
        
        if (targetItem is not null && !ReferenceEquals(draggedItem, targetItem))
        {
            if (dataGrid.ItemsSource is not IList list)
            {
                throw new ApplicationException("EnableRowsMoveProperty requires the ItemsSource property of DataGrid to be at least IList inherited collection. Use ObservableCollection to have movements reflected in UI.");
            }

            var targetIndex = list.IndexOf(targetItem);
            list.Remove(draggedItem);
            
            list.Insert(targetIndex, draggedItem);
        }
    }

    private static T? FindVisualParent<T>(DependencyObject child) where T : DependencyObject
    {
        while (true)
        {
            var parentObject = VisualTreeHelper.GetParent(child);
            
            switch (parentObject)
            {
                case null:
                    return null;
                case T parent:
                    return parent;
                default:
                    child = parentObject;
                    break;
            }
        }
    }

    private static T? TryFindFromPoint<T>(UIElement reference, Point point) where T : DependencyObject
    {
        if (reference.InputHitTest(point) is not DependencyObject element)
        {
            return null;
        }

        if (element is T targetElement)
        {
            return targetElement;
        }

        return FindVisualParent<T>(element);
    }
}
