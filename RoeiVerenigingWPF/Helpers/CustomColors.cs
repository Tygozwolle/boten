#region

using System.Windows.Media;

#endregion

namespace RoeiVerenigingWPF.Helpers;

public abstract class CustomColors
{
    public static SolidColorBrush
        MainBackgroundColor = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)); // #ffffff

    public static SolidColorBrush
        TextBoxBackgroundColor = new SolidColorBrush(Color.FromArgb(255, 232, 246, 252)); // #e8f6fc

    public static SolidColorBrush
        ButtonBackgroundColor = new SolidColorBrush(Color.FromArgb(255, 187, 227, 247)); // #bbe3f7

    public static SolidColorBrush c = new SolidColorBrush(Color.FromArgb(255, 141, 209, 241)); // #8dd1f1
    public static SolidColorBrush d = new SolidColorBrush(Color.FromArgb(255, 95, 190, 236)); // #5fbeec
    public static SolidColorBrush e = new SolidColorBrush(Color.FromArgb(255, 50, 172, 231)); // #32ace7

    public static SolidColorBrush
        OutsideBorderColor = new SolidColorBrush(Color.FromArgb(255, 24, 146, 205)); // #1892cd

    public static SolidColorBrush
        SubHeaderColor = new SolidColorBrush(Color.FromArgb(255, 19, 114, 160)); // #1372a0

    public static SolidColorBrush
        HeaderColor = new SolidColorBrush(Color.FromArgb(255, 14, 81, 114)); // #0e5172

    public static SolidColorBrush i = new SolidColorBrush(Color.FromArgb(255, 8, 49, 68)); // #083144

    public static SolidColorBrush j = new SolidColorBrush(Color.FromArgb(255, 3, 16, 23)); // #031017  
    public static SolidColorBrush DropShadowColor = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)); // #000000  
}