using Algorithm_Chart.Constants;
using System.Threading;
using LiveCharts;
using Algorithm_Chart.Extensions;
using System.Linq;

namespace Algorithm_Chart.Models.Algorithms
{
    public class MergeSort : BaseSortingAlgorithm
    {
        public MergeSort(SortingInfo sortingInfo, NoisyCounter counter, ChartValues<int> dataset) 
            : base(sortingInfo, counter, dataset)
        {
        }

        public override void PopulateTitle()
        {
            this.Title = AlgorithmConstants.MergeSort;
            this.BestCase = $"{Best} nlog(n)";
            this.AverageCase = $"{Average} nlog(n)";
            this.WorstCase = $"{Worst} nlog(n)";
        }

        public override void Sort()
        {
            this.Merge(this.Dataset, 0, this.Dataset.Count - 1);
        }

        private void Merge(ChartValues<int> list, int listStartPoint, int listEndPoint)
        {
            int midPoint = (listEndPoint - listStartPoint) / 2 + listStartPoint;

            if (listStartPoint < midPoint)
            {
                this.Merge(list, listStartPoint, midPoint);
            }

            if (midPoint + 1 < listEndPoint)
            {
                this.Merge(list, midPoint + 1, listEndPoint);
            }

            this.MergeSorter(list, listStartPoint, midPoint, listEndPoint);
        }

        private void MergeSorter(ChartValues<int> list, int startPoint, int midPoint, int endPoint)
        {
            ChartValues<int> tempList = new ChartValues<int>(list.ToArray().DeepClone());
            int leftCounter = startPoint;
            int rightCounter = midPoint + 1;

            for (int i = startPoint; i < endPoint + 1; i++)
            {
                this.Counter.Count++;
                if (this.Token.IsCancellationRequested)
                {
                    return;
                }

                if (leftCounter == midPoint + 1)
                {
                    list[i] = tempList[rightCounter];
                    rightCounter++;
                    continue;
                }

                if (rightCounter == endPoint + 1)
                {
                    list[i] = tempList[leftCounter];
                    leftCounter++;
                    continue;
                }

                if (tempList[leftCounter] <= tempList[rightCounter])
                {
                    list[i] = tempList[leftCounter];
                    leftCounter++;
                }
                else
                {
                    list[i] = tempList[rightCounter];
                    rightCounter++;
                }
                Thread.Sleep(this.SortingInfo.SortingDelay);
            }

            return;
        }
    }
}
