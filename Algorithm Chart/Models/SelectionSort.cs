using System.Threading;
using Algorithm_Chart.Constants;
using LiveCharts;

namespace Algorithm_Chart.Models
{
    public class SelectionSort : IAlgorithm
    {
        public string Title { get; private set; } = AlgorithmConstants.SelectionSort;

        public void Algorithm(SeriesCollection dataSet, int sortingDelay)
        {
            int count = dataSet[0].Values.Count;

            for (int i = 0; i < count; i++)
            {
                int lowestValue = (int)dataSet[0].Values[i];
                int position = i;

                for (int j = i; j < count; j++)
                {
                    if ((int)dataSet[0].Values[j] <= lowestValue)
                    {
                        lowestValue = (int)dataSet[0].Values[j];
                        position = j;
                    }
                    Thread.Sleep(sortingDelay);
                }

                int holder = (int)dataSet[0].Values[i];
                dataSet[0].Values[i] = lowestValue;
                dataSet[0].Values[(int)position] = holder;
            }
        }
    }
}
