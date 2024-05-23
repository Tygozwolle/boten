using System;
using System.Collections.Generic;
using System.Linq;
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
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages.Admin
{
    /// <summary>
    /// Interaction logic for ManageApp.xaml
    /// </summary>
    public partial class ManageApp : Page
    {
        private MainWindow _MainWindow;
        public ManageApp(MainWindow mw)
        {
            _MainWindow = mw;
            InitializeComponent();
        }

        private void Change(object sender, RoutedEventArgs e)
        {

        }
    }
}
