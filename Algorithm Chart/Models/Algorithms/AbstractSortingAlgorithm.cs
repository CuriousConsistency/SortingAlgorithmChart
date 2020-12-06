using System.Threading;
using LiveCharts;

namespace Algorithm_Chart.Models.Algorithms
{
    public abstract class AbstractSortingAlgorithm
    {
        public SortingInfo SortingInfo { get; set; }
        public NoisyCounter Counter { get; set; }
        public string Title { get; internal set; }
        public ChartValues<int> Dataset { get; set; }
        public CancellationToken Token { get; protected set; }
        public string BestCase { get; protected set; }
        public string AverageCase { get; protected set; }
        public string WorstCase { get; protected set; }

        public const string Best = "Best:";
        public const string Average = "Ave:";
        public const string Worst = "Worst:";

        public AbstractSortingAlgorithm(SortingInfo sortingInfo, NoisyCounter counter, ChartValues<int> dataset)
        {
            this.SortingInfo = sortingInfo;
            this.Counter = counter;
            this.Dataset = dataset;
            this.PopulateTitle();
        }

        public abstract void PopulateTitle();

        public abstract void InitialiseSort(CancellationToken token);
    }
}
