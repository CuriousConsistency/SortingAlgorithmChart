using System.Windows;
using Sorting_Algorithm_Chart.ViewModels;

namespace Sorting_Algorithm_Chart.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SortingAlgorithmVisualiser : Window
    {
        public SortingAlgorithmVisualiser()
        {
            this.DataContext = new SortingAlgorithmViewModel();
            InitializeComponent();
        }
    }
}
