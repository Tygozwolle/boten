using System.Windows.Controls;

namespace RoeiVerenigingWPF.Components;

public partial class Header : UserControl
{
    public Header()
    {
        InitializeComponent();
        DataContext = this;
    }
}