using System.Threading;
using System.Threading.Tasks;
using LiveCharts;

namespace Algorithm_Chart.Models.Algorithms
{
    public abstract class BaseSortingAlgorithm
    {
        public SortingInfo SortingInfo { get; set; }
        public NoisyCounter Counter { get; set; }
        public string Title { get; internal set; }
        public ChartValues<int> Dataset { get; set; }
        public CancellationToken Token { get; protected set; }
        public TaskStatus AlgorithmStatus { get; set; }
        public string BestCase { get; protected set; }
        public string AverageCase { get; protected set; }
        public string WorstCase { get; protected set; }

        public const string Best = "Best:";
        public const string Average = "Ave:";
        public const string Worst = "Worst:";

        public BaseSortingAlgorithm(SortingInfo sortingInfo, NoisyCounter counter, ChartValues<int> dataset)
        {
            this.SortingInfo = sortingInfo;
            this.Counter = counter;
            this.Dataset = dataset;
            this.AlgorithmStatus = TaskStatus.WaitingForActivation;
            this.PopulateTitle();
        }

        public abstract void PopulateTitle();

        public void InitialiseSort(CancellationToken token)
        {
            this.Token = token;
            this.AlgorithmStatus = TaskStatus.Running;
            this.Sort();
            this.AlgorithmStatus = TaskStatus.RanToCompletion;
        }

        public abstract void Sort();
    }
}
