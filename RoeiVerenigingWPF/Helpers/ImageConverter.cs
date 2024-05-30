﻿using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RoeiVerenigingWPF.Helpers;

public abstract class ImageConverter
{
    public static ImageSource Convert(Bitmap bitmap)
    {
        using (var memory = new MemoryStream())
        {
            bitmap.Save(memory, ImageFormat.Bmp);
            memory.Position = 0;
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }

    public static ImageSource Convert(Stream stream)
    {
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = stream;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();
        return bitmapImage;
    }

    public static List<ImageSource> ConvertList(List<Stream> streams)
    {
        if (streams == null) return new List<ImageSource>();

        var ImageSources = new List<ImageSource>(streams.Count);

        foreach (var stream in streams) ImageSources.Add(Convert(stream));
        return ImageSources;
    }
}