using QRCoder;
using RoeiVerenigingLibary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace RoeiVerenigingWPF
{
    public abstract class QrcodeMaker
    {
        public static ImageSource qrcode(int id)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(EmailToDb.GetStringForEmail(id), QRCodeGenerator.ECCLevel.H))
                {
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        using (Bitmap qrCodeBitmap = qrCode.GetGraphic(20))
                        {
                            return ImageConverter.Convert(qrCodeBitmap);
                        }
                    }
                }
            }
        }
    }
}
