using RoeiVerenigingWPF.Frames;
using System.Windows.Controls;
using System.Windows.Data;

namespace RoeiVerenigingWPF.Components;

public partial class Header : UserControl
{
    public MainWindow main { set; get; }
    public Header()
    {
        InitializeComponent();
        DataContext = this;
        Binding binding = new Binding(null);
       binding.Source = main;
        binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        ___Name_.SetBinding(ComboBoxItem.ContentProperty, binding);
    }
}