using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RoeiVerenigingWPF.helpers;

namespace RoeiVerenigingWPF
{
    [ValueConversion(typeof(List<Stream>), typeof(ImageSource))]
    public class ImageSourceConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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
                return new BitmapImage(new Uri("/Images/Image_not_available.png.png", UriKind.Relative));
            }
            else
            {
                if (list[0] == null)
                {
                    return new BitmapImage(new Uri("/Images/Image_not_available.png.png", UriKind.Relative));
                }
                else
                {
                    return ImageConverter.Convert(list[0]);
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
