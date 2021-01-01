using System.Windows;
using Sorting_Algorithm_Chart.Views;
using Sorting_Algorithm_Chart.ViewModels;

namespace Sorting_Algorithm_Chart
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SortingAlgorithmVisualiser sortingAlgorithmVisualiser = new SortingAlgorithmVisualiser();
            SortingAlgorithmViewModel sortingAlgorithmViewModel = new SortingAlgorithmViewModel();
            Application.Current.MainWindow.Closing += sortingAlgorithmViewModel.OnWindowClosing;
            sortingAlgorithmVisualiser.DataContext = sortingAlgorithmViewModel;
            sortingAlgorithmVisualiser.Show();
        }
    }
}
