using QRCoder;
using RoeiVerenigingLibary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    /// Interaction logic for QRCode.xaml
    /// </summary>
    public partial class QRCodePage : Page
    {
        private int _damageID;
        public QRCodePage(int damageID)
        {
            _damageID = damageID;
   
            
            InitializeComponent();
           
            QrcodeImage.Source = QrcodeMaker.qrcode(_damageID);
        }
        
    }
}
