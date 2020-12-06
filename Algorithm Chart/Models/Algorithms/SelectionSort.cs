﻿using Algorithm_Chart.Constants;
using System.Threading;
using LiveCharts;

namespace Algorithm_Chart.Models.Algorithms
{
    public class SelectionSort : AbstractSortingAlgorithm
    {
        public SelectionSort(SortingInfo sortingInfo, NoisyCounter counter, ChartValues<int> dataset) : base(sortingInfo, counter, dataset)
        {
        }

        public override void PopulateTitle()
        {
            this.Title = AlgorithmConstants.SelectionSort;
            this.BestCase = $"{Best} n^2";
            this.AverageCase = $"{Average} n^2";
            this.WorstCase = $"{Worst} n^2";
        }

        public override void InitialiseSort(CancellationToken token)
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

                    if (token.IsCancellationRequested)
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
