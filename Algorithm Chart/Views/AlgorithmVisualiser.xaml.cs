using System.Windows;
using Algorithm_Chart.ViewModels;

namespace Algorithm_Chart.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AlgorithmVisualiser : Window
    {
        public AlgorithmVisualiser()
        {
            this.DataContext = new AlgorithmViewModel();
            InitializeComponent();
        }
    }
}
