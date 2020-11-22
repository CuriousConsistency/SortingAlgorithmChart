using System;
using System.Collections.Generic;
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
            AlgorithmViewModel algorithmViewModel = new AlgorithmViewModel();
            Application.Current.MainWindow.Closing += algorithmViewModel.OnWindowClosing;
            algorithmVisualiser.DataContext = algorithmViewModel;
            algorithmVisualiser.Show();
        }
    }
}
