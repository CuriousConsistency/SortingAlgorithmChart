using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Algorithm_Chart.Views;
using Algorithm_Chart.ViewModels;

namespace Algorithm_Chart
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AlgorithmVisualiser algorithmVisualiser = new AlgorithmVisualiser();
            algorithmVisualiser.DataContext = new AlgorithmViewModel();
            algorithmVisualiser.Show();
        }
    }
}
