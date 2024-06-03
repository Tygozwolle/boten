using System.Globalization;
using System.Windows.Data;

namespace RoeiVerenigingWPF;

public class TimeSpanToDateTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeSpan)
        {
            return new DateTime().Date + timeSpan;
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            return dateTime.TimeOfDay;
        }
        return value;
    }
}