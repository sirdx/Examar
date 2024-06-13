using System.Windows.Controls;

namespace Examar.Views.Creator;

public partial class CreatorView
{
    public CreatorView()
    {
        InitializeComponent();
    }

    private void DataGrid_OnLoadingRow(object? sender, DataGridRowEventArgs e)
    {
        e.Row.Header = (e.Row.GetIndex() + 1).ToString();
    }
}
