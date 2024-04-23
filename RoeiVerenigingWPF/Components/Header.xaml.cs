using RoeiVerenigingWPF.Frames;
using System.Windows.Controls;

namespace RoeiVerenigingWPF.Components;

public partial class Header : UserControl
{
    public MainWindow main { set; get; }
    public Header()
    {
        InitializeComponent();
        DataContext = this;
    }
}