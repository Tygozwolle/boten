using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RoeiVerenigingWPF.Pages
{
    [ValueConversion(typeof(DateTime), typeof(String))]
public class PropertyValueStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
            DateTime castValue = (DateTime)value;
        return castValue.ToString("dd-MM-yyyy HH:mm:ss");
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

}