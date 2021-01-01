using System;
using System.Windows;
using System.Windows.Data;

namespace Sorting_Algorithm_Chart.Converters
{
    public class BoolToVisiblityConverter : IValueConverter
    {
        public object Convert(object available, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool show = (bool)available;
            if (show)
            {
                return Visibility.Visible;
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object available, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
