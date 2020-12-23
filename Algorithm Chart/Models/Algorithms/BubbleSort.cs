using Algorithm_Chart.Constants;
using System.Threading;
using System.Threading.Tasks;
using LiveCharts;

namespace Algorithm_Chart.Models.Algorithms
{
    public class BubbleSort : BaseSortingAlgorithm
    {
        public BubbleSort(SortingInfo sortingInfo, NoisyCounter counter, ChartValues<int> dataset) 
            : base(sortingInfo, counter, dataset)
        {
        }

        public override void PopulateTitle()
        {
            this.Title = AlgorithmConstants.BubbleSort;
            this.BestCase = $"{Best} n^2";
            this.AverageCase = $"{Average} n^2";
            this.WorstCase = $"{Worst} n";
        }

        public override void Sort()
        {
            int count = this.Dataset.Count;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count - i - 1; j++)
                {
                    this.Counter.Count++;
                    if (this.Dataset[j] >= this.Dataset[j + 1])
                    {
                        int holder = this.Dataset[j + 1];
                        this.Dataset[j + 1] = this.Dataset[j];
                        this.Dataset[j] = holder;
                        Thread.Sleep(this.SortingInfo.SortingDelay);
                    }

                    if (this.AlgorithmStatus == TaskStatus.WaitingToRun)
                    {
                        while(this.AlgorithmStatus != TaskStatus.Running)
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
