using System.Globalization;
using System.Windows.Data;

namespace Examar.Converters;

public class StringResourceConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string key)
        {
            return string.Empty;
        }
        
        return Resources.ResourceManager.GetString(key) ?? string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
