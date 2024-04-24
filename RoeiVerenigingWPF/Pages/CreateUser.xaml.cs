using DataAccessLibary;
using RoeiVerenigingLibary.Exceptions;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;
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

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    /// Interaction logic for CreateUser.xaml
    /// </summary>
    public partial class CreateUser : Page
    {
        private MainWindow _mainWindow;
        public CreateUser(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

    }
}
