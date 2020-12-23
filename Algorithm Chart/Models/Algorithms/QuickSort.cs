using Algorithm_Chart.Constants;
using System.Threading;
using LiveCharts;

namespace Algorithm_Chart.Models.Algorithms
{
    public class QuickSort : BaseSortingAlgorithm
    {
        public QuickSort(SortingInfo sortingInfo, NoisyCounter counter, ChartValues<int> dataset) 
            : base(sortingInfo, counter, dataset)
        {
        }

        public override void PopulateTitle()
        {
            this.Title = AlgorithmConstants.QuickSort;
            this.BestCase = $"{Best} n^2";
            this.AverageCase = $"{Average} nlog(n)";
            this.WorstCase = $"{Worst} nlog(n)";
        }

        public override void Sort()
        {
            this.Sort(this.Dataset, 0 , this.Dataset.Count - 1);
        }

        public void Sort(ChartValues<int> list, int start, int end)
        {
            if (end - start <= 0)
            {
                return;
            }

            int pivotIndex = end;
            int loopCount = start;
            int smallerElement = start - 1;

            while (loopCount < pivotIndex)
            {
                if (list[loopCount] < list[pivotIndex])
                {
                    smallerElement++;
                    this.Swap(list, loopCount, smallerElement);
                    Thread.Sleep(this.SortingInfo.SortingDelay);
                }

                this.Counter.Count++;
                loopCount++;

                if (this.Token.IsCancellationRequested)
                {
                    return;
                }
            }

            this.Swap(list, smallerElement + 1, pivotIndex);
            Thread.Sleep(this.SortingInfo.SortingDelay);

            this.Sort(list, start, smallerElement);
            this.Sort(list, smallerElement + 2, loopCount);
        }

        public void Swap(ChartValues<int> list, int a, int b)
        {
            if (a != b)
            {
                int temp = list[a];
                list[a] = list[b];
                list[b] = temp;
            }
        }
    }
}
