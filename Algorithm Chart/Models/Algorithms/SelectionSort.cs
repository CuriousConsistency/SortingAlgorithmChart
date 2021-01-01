using Sorting_Algorithm_Chart.Constants;
using System.Threading;
using System.Threading.Tasks;
using LiveCharts;

namespace Sorting_Algorithm_Chart.Models.Algorithms
{
    public class SelectionSort : BaseSortingAlgorithm
    {
        public SelectionSort(SortingInfo sortingInfo, NoisyCounter counter, ChartValues<int> dataset) 
            : base(sortingInfo, counter, dataset)
        {
        }

        public override void PopulateTitle()
        {
            this.Title = AlgorithmConstants.SelectionSort;
            this.BestCase = $"{Best} n^2";
            this.AverageCase = $"{Average} n^2";
            this.WorstCase = $"{Worst} n^2";
        }

        public override void Sort()
        {
            int count = this.Dataset.Count;
            for (int i = 0; i < count; i++)
            {
                int lowestValue = this.Dataset[i];
                int position = i;
                bool sortOccur = false;

                for (int j = i; j < count; j++)
                {
                    this.Counter.Count++;
                    if (this.Dataset[j] <= lowestValue)
                    {
                        lowestValue = this.Dataset[j];
                        position = j;
                        sortOccur = true;
                    }

                    if (this.AlgorithmStatus == TaskStatus.WaitingToRun)
                    {
                        while (this.AlgorithmStatus != TaskStatus.Running)
                        {
                            Task.Delay(50);
                        }
                    }

                    if (this.Token.IsCancellationRequested)
                    {
                        return;
                    }
                }

                if (sortOccur)
                {
                    int holder = this.Dataset[i];
                    this.Dataset[i] = lowestValue;
                    this.Dataset[position] = holder;
                    Thread.Sleep(this.SortingInfo.SortingDelay);
                }
            }
        }
    }
}
