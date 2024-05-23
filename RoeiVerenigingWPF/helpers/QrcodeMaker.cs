﻿using QRCoder;
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
using RoeiVerenigingLibrary;
using Color = System.Drawing.Color;

namespace RoeiVerenigingWPF.helpers
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
                        using (Bitmap qrCodeBitmap = qrCode.GetGraphic(20, Color.Black,  Color.FromArgb(232, 246,252),  new Bitmap("Images/logo.png"), 25,0,true,null))
                        { 
                            return ImageConverter.Convert(qrCodeBitmap);
                        }
                    }
                }
            }
        }
    }
}
