using System.Threading;
using Algorithm_Chart.Constants;
using LiveCharts;

namespace Algorithm_Chart.Models
{
    public class BubbleSort : IAlgorithm
    {
        public string Title { get; private set; } = AlgorithmConstants.BubbleSort;

        public void Algorithm(SeriesCollection dataSet, int sortingDelay)
        {
            int count = dataSet[0].Values.Count;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count - i - 1; j++)
                {
                    if ((int)dataSet[0].Values[j] >= (int)dataSet[0].Values[j + 1])
                    {
                        int holder = (int)dataSet[0].Values[j + 1];
                        dataSet[0].Values[j + 1] = (int)dataSet[0].Values[j];
                        dataSet[0].Values[j] = holder;
                    }
                    Thread.Sleep(sortingDelay);
                }
            }
        }
    }
}
