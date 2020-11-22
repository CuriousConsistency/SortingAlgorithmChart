using System.Windows;
using System.Windows.Controls;

namespace Algorithm_Chart.UserControls
{
    /// <summary>
    /// Interaction logic for AlgorithmBarChart.xaml
    /// </summary>
    public partial class AlgorithmBarChart : UserControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object),
            typeof(AlgorithmBarChart), new PropertyMetadata(null));

        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public AlgorithmBarChart()
        {
            InitializeComponent();
            this.chart.DataContext = this;
            this.IsEnabled = false;
        }
    }
}
