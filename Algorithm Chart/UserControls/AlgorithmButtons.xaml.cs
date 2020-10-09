using System.Windows;
using System.Windows.Controls;

namespace Algorithm_Chart.UserControls
{
    /// <summary>
    /// Interaction logic for AlgorithmButtons.xaml
    /// </summary>
    public partial class AlgorithmButtons : UserControl
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(object),
            typeof(AlgorithmButtons), new PropertyMetadata(null));

        public object Command
        {
            get { return (object)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public AlgorithmButtons()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
