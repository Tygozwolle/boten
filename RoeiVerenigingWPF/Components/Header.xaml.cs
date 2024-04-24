using RoeiVerenigingWPF.Frames;
using System.Windows.Controls;
using System.Windows.Data;

namespace RoeiVerenigingWPF.Components;

public partial class Header : UserControl
{
    public MainWindow MainWindow { set; get; }
    public Header()
    {
        InitializeComponent();
        DataContext = this;
    }
}