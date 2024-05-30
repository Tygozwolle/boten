#region

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

#endregion

namespace RoeiVerenigingWPF.Helpers;

public abstract class ResizeImage //needs to be in WPF, Otherwise it will not work
{
    public static Stream ResizeTheImage(Stream imageStream, int width, int height)
    {
        var destRect = new Rectangle(0, 0, width, height);
        var destImage = new Bitmap(width, height);
        using (Image image = Image.FromStream(imageStream))
        {
            var imageHeight = image.Height;
            var imageWidth = image.Width;
            if (imageWidth > width || imageHeight > height)
            {
                if (imageWidth > imageHeight)
                {
                    destRect.Height = (int)(height * ((double)imageHeight / imageWidth));
                }
                else
                {
                    destRect.Width = (int)(width * ((double)imageWidth / imageHeight));
                }
            }
            else
            {
                destRect.Width = imageWidth;
                destRect.Height = imageHeight;
            }

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel,
                        wrapMode);
                }
            }

            MemoryStream ms = new MemoryStream();
            destImage.Save(ms, ImageFormat.Png);

            return ms;
        }
    }
}