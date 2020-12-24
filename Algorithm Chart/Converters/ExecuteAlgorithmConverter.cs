using System;
using System.Windows.Data;
using System.Threading.Tasks;

namespace Algorithm_Chart.Converters
{
    public class ExecuteAlgorithmConverter : IMultiValueConverter
    {
        public object Convert(object[] objects, Type type, object parameter, System.Globalization.CultureInfo cultureInfo)
        {
            string selectedAlgorithm = objects[0].ToString();
            string executingAlgorithm = objects[1].ToString();
            TaskStatus taskStatus = (TaskStatus)objects[2];

            if ((selectedAlgorithm == executingAlgorithm && 
                (taskStatus == TaskStatus.WaitingToRun || taskStatus == TaskStatus.RanToCompletion || taskStatus == TaskStatus.Faulted))
                || (selectedAlgorithm != executingAlgorithm))
            {
                return "Execute";
            }

            return "Pause";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
