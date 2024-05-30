using System.Windows.Controls;

namespace RoeiVerenigingWPF.Pages.Admin;

public partial class ViewTrends : Page
{
    
    
    public ViewTrends()
    {
        InitializeComponent();
        var x = Enumerable.Range(0, 1001).Select(i => i / 10.0).ToArray();
        var y = x.Select(v => Math.Abs(v) < 1e-10 ? 1 : Math.Sin(v) / v).ToArray();
        bars.Plot(x); // x and y are IEnumerable<double>
    }
    
}