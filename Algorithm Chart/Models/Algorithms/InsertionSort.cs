using Sorting_Algorithm_Chart.Constants;
using System.Threading;
using System.Threading.Tasks;
using LiveCharts;

namespace Sorting_Algorithm_Chart.Models.Algorithms
{
    public class InsertionSort : BaseSortingAlgorithm
    {
        public InsertionSort(SortingInfo sortingInfo, NoisyCounter counter, ChartValues<int> dataset) 
            : base(sortingInfo, counter, dataset)
        {
        }

        public override void PopulateTitle()
        {
            this.Title = AlgorithmConstants.InsertionSort;
            this.BestCase = $"{Best} n^2";
            this.AverageCase = $"{Average} n^2";
            this.WorstCase = $"{Worst} n";
        }

        public override void Sort()
        {
            int count = this.Dataset.Count;

            for (int i = 1; i < count; i++)
            {
                int j = i;
                while (j > 0 && this.Dataset[j] < this.Dataset[j - 1])
                {
                    this.Counter.Count++;
                    int temp = this.Dataset[j];
                    this.Dataset[j] = this.Dataset[j - 1];
                    this.Dataset[j - 1] = temp;
                    Thread.Sleep(this.SortingInfo.SortingDelay);
                    j--;

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
            }
        }
    }
}
