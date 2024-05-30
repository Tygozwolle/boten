#region

using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RoeiVerenigingWPF.Helpers;

#endregion

namespace RoeiVerenigingWPF.Converters;

[ValueConversion(typeof(List<Stream>), typeof(ImageSource))]
public class ImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            if (value == null)
            {
                return new BitmapImage(new Uri("/Images/Image_not_available.png", UriKind.Relative));
            }
            //if (targetType != typeof(List<Stream>))
            //{
            //    throw new InvalidOperationException("The target must be a List<Stream>");
            //}

            var list = (List<Stream>)value;
            if (list.Count == 0)
            {
                return new BitmapImage(new Uri("/Images/Image_not_available.png", UriKind.Relative));
            }

            if (list[0] == null)
            {
                return new BitmapImage(new Uri("/Images/Image_not_available.png", UriKind.Relative));
            }

            return ImageConverter.Convert(list[0]);
        }
        catch
        {
            return new BitmapImage(new Uri("/Images/Image_not_available.png", UriKind.Relative));
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}