using RoeiVerenigingWPF.helpers;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RoeiVerenigingWPF
{
    [ValueConversion(typeof(Stream), typeof(ImageSource))]
    public class SingleStreamImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    return new BitmapImage(new Uri("/Images/Image_not_available.png", UriKind.Relative));
                }

                return ImageConverter.Convert(value as Stream);
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
}