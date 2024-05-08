using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DataAccessLibary;
using RoeiVerenigingLibary;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    /// Interaction logic for ListBoats.xaml
    /// </summary>
    /// 

    public partial class ListBoats : Page
    {
        public List<Boat> boats { get; set; }

        public ListBoats()
        {
            InitializeComponent();
            BoatService service = new BoatService(new BoatRepository());
            this.DataContext = this;

            boats = service.Get();


        }

        private void StackPanel_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is StackPanel)
            {
                StackPanel casted = sender as StackPanel;
            }
        }
    }
}
