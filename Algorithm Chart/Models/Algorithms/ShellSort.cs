using Algorithm_Chart.Constants;
using System.Threading;
using LiveCharts;

namespace Algorithm_Chart.Models.Algorithms
{
    public class ShellSort : AbstractSortingAlgorithm
    {
        public ShellSort(SortingInfo sortingInfo, NoisyCounter counter, ChartValues<int> dataset) : base(sortingInfo, counter, dataset)
        {
        }

        public override void PopulateTitle()
        {
            this.Title = AlgorithmConstants.ShellSort;
            this.BestCase = $"{Best} var";
            this.AverageCase = $"{Average} n^(3/2)";
            this.WorstCase = $"{Worst} n";
        }

        public override void InitialiseSort(CancellationToken token)
        {
            this.Token = token;
            this.Sort(this.Dataset.Count > 15 == true ? 5 : 3);
        }

        public void Sort(int gap)
        {
            this.InsertionSort(gap);
            if (gap > 1)
            {
                this.Sort(gap -= 2);
            }        
        }

        public void InsertionSort(int gap)
        {
            int count = this.Dataset.Count;

            for (int i = gap; i < count; i += gap)
            {
                int j = i;
                while (j > 0 && this.Dataset[j] < this.Dataset[j - gap])
                {
                    this.Counter.Count++;
                    int temp = this.Dataset[j];
                    this.Dataset[j] = this.Dataset[j - 1];
                    this.Dataset[j - 1] = temp;
                    Thread.Sleep(this.SortingInfo.SortingDelay);
                    j -= gap;

                    if (this.Token.IsCancellationRequested)
                    {
                        return;
                    }
                }
            }
        }
    }
}
