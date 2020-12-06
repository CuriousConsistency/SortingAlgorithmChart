using Algorithm_Chart.Constants;
using System.Threading;
using LiveCharts;

namespace Algorithm_Chart.Models.Algorithms
{
    public class InsertionSort : AbstractSortingAlgorithm
    {
        public InsertionSort(SortingInfo sortingInfo, NoisyCounter counter, ChartValues<int> dataset) : base(sortingInfo, counter, dataset)
        {
        }

        public override void PopulateTitle()
        {
            this.Title = AlgorithmConstants.InsertionSort;
            this.BestCase = $"{Best} n^2";
            this.AverageCase = $"{Average} n^2";
            this.WorstCase = $"{Worst} n";
        }

        public override void InitialiseSort(CancellationToken token)
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

                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                }
            }
        }
    }
}
