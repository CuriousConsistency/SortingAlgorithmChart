using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;
using Algorithm_Chart.Commands;
using Algorithm_Chart.Extensions;
using Algorithm_Chart.Models;
using Algorithm_Chart.Models.Algorithms;
using System.Linq;

namespace Algorithm_Chart.ViewModels
{
    public class AlgorithmViewModel
    {
        public SeriesCollection DataSet { get; private set; }
        public List<string> AlgorithmNames { get; private set; }
        public List<BaseSortingAlgorithm> Algorithms { get; private set; }
        public int MinimumArraySize { get; private set; }
        public int MaximumArraySize { get; private set; }
        public int ArraySize { get; set; }
        public int MaximumSortingDelay { get; private set; }
        public SortingInfo SortingInfo { get; private set; }
        public int MinimumSortingDelay { get; private set; }
        public bool GeneratingData { get; private set; }
        public NoisyTimer Timer { get; private set; }
        public NoisyCounter Counter { get; private set; }
        public AlgorithmStatistics AlgorithmStatistics { get; private set; }
        public RelayCommand ExecuteAlgorithmCommand
        {
            get
            {
               return new RelayCommand(() => !this.GeneratingData, AlgorithmHandler);
            }
        }
        public RelayCommand GenerateNewDataSetCommand
        {
            get
            {
                return new RelayCommand(() => !this.GeneratingData, this.DataSetGeneratorHandler);
            }
        }

        private readonly int timerInterval;
        private readonly int upperBound;
        private ChartValues<int> unsortedDataset;
        private CancellationTokenSource tokenSource;
        private CancellationToken token;
        private TaskStatus algorithmStatus;
        private Task task;
        private string executingAlgorithm;

        public AlgorithmViewModel()
        {
            this.DataSet = new SeriesCollection
            {
                new ColumnSeries()
            };

            this.MinimumSortingDelay = 0;
            this.MaximumSortingDelay = 1000;
            this.SortingInfo = new SortingInfo(false, 500);
            this.MinimumArraySize = 10;
            this.MaximumArraySize = 50;
            this.ArraySize = 20;
            this.Algorithms = new List<BaseSortingAlgorithm>();
            this.AlgorithmNames = new List<string>();
            this.Counter = new NoisyCounter();
            this.timerInterval = 50;
            this.Timer = new NoisyTimer(this.timerInterval);
            this.AlgorithmStatistics = new AlgorithmStatistics();
            this.algorithmStatus = TaskStatus.WaitingForActivation;
            this.upperBound = 100;

            this.DataSetGenerator();
            this.PopulateAlgorithms();
            this.PopulateAlgorithmNames();
        }

        public void PopulateAlgorithms()
        {
            this.Algorithms.Add(new BubbleSort(this.SortingInfo, this.Counter, (ChartValues<int>)this.DataSet[0].Values));
            this.Algorithms.Add(new SelectionSort(this.SortingInfo, this.Counter, (ChartValues<int>)this.DataSet[0].Values));
            this.Algorithms.Add(new MergeSort(this.SortingInfo, this.Counter, (ChartValues<int>)this.DataSet[0].Values));
            this.Algorithms.Add(new InsertionSort(this.SortingInfo, this.Counter, (ChartValues<int>)this.DataSet[0].Values));
            this.Algorithms.Add(new ShellSort(this.SortingInfo, this.Counter, (ChartValues<int>)this.DataSet[0].Values));
            this.Algorithms.Add(new QuickSort(this.SortingInfo, this.Counter, (ChartValues<int>)this.DataSet[0].Values));
        }

        public void PopulateAlgorithmNames()
        {
            this.Algorithms.ForEach(a => this.AlgorithmNames.Add(a.Title));
        }

        public async Task TaskHandler()
        {
            try
            {
                if (this.task is null)
                {
                    this.SetTasksAndCancellationToken();
                    return;
                }

                if (this.task.Status == TaskStatus.Faulted || 
                    this.task.Status == TaskStatus.RanToCompletion)
                {
                    this.SetTasksAndCancellationToken();
                    return;
                }

                if (!this.SortingInfo.Sorted)
                {
                    this.algorithmStatus = TaskStatus.Running;
                    this.Algorithms.Find(a => a.Title == this.executingAlgorithm).AlgorithmStatus = this.algorithmStatus;
                    this.tokenSource.Cancel();
                }

                await this.task;
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                this.SetTasksAndCancellationToken();
                this.ResetStatistics();
            }
        }

        public void SetTasksAndCancellationToken()
        {
            this.tokenSource = new CancellationTokenSource();
            this.token = this.tokenSource.Token;
        }

        public async void DataSetGeneratorHandler(object unused)
        {
            await this.TaskHandler();
            this.DataSetGenerator();
        }

        public void DataSetGenerator()
        {
            this.GeneratingData = true;
            Random rand = new Random();
            this.unsortedDataset = new ChartValues<int>();

            for (int i = 0; i < this.ArraySize; i++)
            {
                this.unsortedDataset.Add(rand.Next(0, this.upperBound));
            }

            this.CloneUnsortedDataset();
            this.GeneratingData = false;
        }

        public void CloneUnsortedDataset()
        {
            this.DataSet[0].Values = new ChartValues<int>(this.unsortedDataset.ToArray().DeepClone());
            this.SortingInfo.Sorted = false;
        }

        public void CloneUnsortedDatasetIfSorted()
        {
            if (this.SortingInfo.Sorted)
            {
                this.CloneUnsortedDataset();
            }
        }

        public void ResetStatistics()
        {
            this.Counter.ResetCount();
            this.Timer.ResetTimer();
            this.AlgorithmStatistics.ResetStatistics();
        }

        public void SetAlgorithmStatistics(BaseSortingAlgorithm sortingAlgorithm)
        {
            this.AlgorithmStatistics.BestCase = sortingAlgorithm.BestCase;
            this.AlgorithmStatistics.AverageCase = sortingAlgorithm.AverageCase;
            this.AlgorithmStatistics.WorstCase = sortingAlgorithm.WorstCase;
        }

        public async void AlgorithmHandler(object parmameter)
        {
            string algorithm = parmameter.ToString();

            if (this.algorithmStatus == TaskStatus.Running && this.task != null && algorithm == this.executingAlgorithm)
            {
                this.algorithmStatus = TaskStatus.WaitingToRun;
                this.Algorithms.Find(a => a.Title == algorithm).AlgorithmStatus = this.algorithmStatus;
                this.Timer.PauseTimer();
                return;
            }

            if (this.algorithmStatus == TaskStatus.WaitingToRun && this.task != null && algorithm == this.executingAlgorithm)
            {
                this.algorithmStatus = TaskStatus.Running;
                this.Algorithms.Find(a => a.Title == algorithm).AlgorithmStatus = this.algorithmStatus;
                this.Timer.ResumeTimer();
                return;
            }

            await this.TaskHandler();
            this.CloneUnsortedDatasetIfSorted();
            this.SortingInfo.Sorted = false;

            BaseSortingAlgorithm sortingAlgorithm = this.Algorithms.Find(a => a.Title == algorithm);
            this.executingAlgorithm = sortingAlgorithm.Title;
            sortingAlgorithm.Dataset = (ChartValues<int>)this.DataSet[0].Values;
            this.SetAlgorithmStatistics(sortingAlgorithm);

            await this.AlgorithmRunner(sortingAlgorithm.InitialiseSort);
            this.SortingInfo.Sorted = true;
            this.executingAlgorithm = string.Empty;
        }

        private async Task AlgorithmRunner(Action<CancellationToken> algorithm)
        {
            try
            {
                this.task = new Task(() => algorithm(this.token), this.token);
                this.Timer.StartTimer();
                this.task.Start();
                this.algorithmStatus = TaskStatus.Running;
                await this.task;
                this.Timer.EndTimer();
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                this.algorithmStatus = TaskStatus.RanToCompletion;
                this.tokenSource.Dispose();
            }
        }

        public async void OnWindowClosing(object sender, EventArgs e)
        {
            await this.TaskHandler();

            if (this.tokenSource != null)
            {
                this.tokenSource.Dispose();
            }

            this.Timer.Close();
        }
    }
}
