#region

using System.Globalization;
using System.Windows.Data;

#endregion

namespace RoeiVerenigingWPF.Pages;

[ValueConversion(typeof(DateTime), typeof(string))]
public class PropertyValueStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        DateTime castValue = (DateTime)value;
        return castValue.ToString("dd-MM-yyyy HH:mm:ss");
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}