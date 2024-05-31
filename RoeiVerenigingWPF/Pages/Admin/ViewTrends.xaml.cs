﻿using System.Windows;
using System.Windows.Controls;

namespace RoeiVerenigingWPF.Pages.Admin;

public partial class ViewTrends : Page
{
    
    
    public ViewTrends()
    {
        InitializeComponent();
        var x = Enumerable.Range(0, 1001).Select(i => i / 10.0).ToArray();
        var y = x.Select(v => Math.Abs(v) < 1e-10 ? 1 : Math.Sin(v) / v).ToArray();
        LineGraph.Plot(x,y); // x and y are IEnumerable<double>
    }

    public static int[] Value = Enumerable.Range(0, 1001).Select(i => i / 10).ToArray();
    public double[] Value2 = Value.Select(v => Math.Abs(v)< 1e-10 ? 1 : Math.Sin(v) / v).ToArray();
    
    
    public void GetBesteResultaat()
    {
        var highestValue = Value.GetUpperBound(1000);
        BestScore.Content = $"Beste: \n {highestValue}";
        BestScore.Visibility = Visibility.Visible;
    }
    
}