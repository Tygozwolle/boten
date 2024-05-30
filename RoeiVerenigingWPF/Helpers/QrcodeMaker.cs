#region

using System.Drawing;
using System.Windows.Media;
using QRCoder;
using RoeiVerenigingLibrary;
using Color = System.Drawing.Color;

#endregion

namespace RoeiVerenigingWPF.Helpers;

public abstract class QrcodeMaker
{
    public static ImageSource Qrcode(int id)
    {
        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        {
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(EmailToDb.GetStringForEmail(id), QRCodeGenerator.ECCLevel.H))
            {
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    using (Bitmap qrCodeBitmap = qrCode.GetGraphic(20, Color.Black, Color.FromArgb(232, 246, 252), new Bitmap("Images/logo.png"), 25))
                    {
                        return ImageConverter.Convert(qrCodeBitmap);
                    }
                }
            }
        }
    }
}