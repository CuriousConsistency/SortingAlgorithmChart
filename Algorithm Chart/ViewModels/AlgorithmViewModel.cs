using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using Algorithm_Chart.Commands;
using Algorithm_Chart.Constants;
using Algorithm_Chart.Extensions;
using Algorithm_Chart.Models;
using System.Linq;

namespace Algorithm_Chart.ViewModels
{
    public class AlgorithmViewModel
    {
        public SeriesCollection DataSet { get; private set; }
        public List<string> Algorithms { get; private set; }
        public int MinimumArraySize { get; private set; }
        public int MaximumArraySize { get; private set; }
        public int ArraySize { get; set; }
        public int MaximumSortingDelay { get; private set; }
        public int SortingDelay { get; set; }
        public int MinimumSortingDelay { get; private set; }
        public bool GeneratingData { get; private set; }
        public bool Sorted { get; private set; }
        public NoisyTimer Timer { get; private set; }
        public NoisyCounter Counter { get; private set; }
        public ICommand ExecuteAlgorithmCommand { get; private set; }
        public RelayCommand GenerateNewDataSetCommand
        {
            get
            {
                return new RelayCommand(() => !this.GeneratingData, this.DataSetGeneratorHandler);
            }
        }

        private readonly int _TimerInterval;
        private readonly int _UpperBound;
        private ChartValues<int> _UnsortedDataset;
        private CancellationTokenSource _TokenSource;
        private CancellationToken _Token;
        private ConcurrentBag<Task> _Tasks;

        public AlgorithmViewModel()
        {
            this.DataSet = new SeriesCollection
            {
                new ColumnSeries()
            };

            this.MinimumSortingDelay = 0;
            this.MaximumSortingDelay = 1000;
            this.SortingDelay = 500;
            this.MinimumArraySize = 10;
            this.MaximumArraySize = 50;
            this.ArraySize = 20;
            this.Algorithms = new List<string>();
            this.ExecuteAlgorithmCommand = new RelayCommand(() => !this.GeneratingData, AlgorithmHandler);
            this._Tasks = new ConcurrentBag<Task>();
            this._UpperBound = 100;
            this._TimerInterval = 50;
            this.Timer = new NoisyTimer(this._TimerInterval);
            this.Counter = new NoisyCounter();
            this.DataSetGenerator();
            this.PopulateAlgorithms();
        }

        public void PopulateAlgorithms()
        {
            this.Algorithms.Add(AlgorithmConstants.BubbleSort);
            this.Algorithms.Add(AlgorithmConstants.SelectionSort);
            this.Algorithms.Add(AlgorithmConstants.MergeSort);
            this.Algorithms.Add(AlgorithmConstants.InsertionSort);
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
            this._UnsortedDataset = new ChartValues<int>();

            for (int i = 0; i < this.ArraySize; i++)
            {
                this._UnsortedDataset.Add(rand.Next(0, this._UpperBound));
            }

            this.CloneUnsortedDataset();
            this.GeneratingData = false;
        }

        public void CloneUnsortedDataset()
        {
            this.DataSet[0].Values = new ChartValues<int>(this._UnsortedDataset.ToArray().DeepClone());
            this.Sorted = false;
        }

        public void CloneUnsortedDatasetIfSorted()
        {
            if (this.Sorted)
            {
                this.CloneUnsortedDataset();
            }
        }

        public void ResetStatistics()
        {
            this.Counter.ResetCount();
            this.Timer.ResetTimer();
        }

        public async Task TaskHandler()
        {
            try
            {
                if (this._Tasks.Count == 0)
                {
                    this.SetTasksAndCancellationToken();
                    return;
                }

                if (!this.Sorted)
                {
                    this._TokenSource.Cancel();
                }
                await this._Tasks.First<Task>();
            }
            catch (OperationCanceledException)
            {
            }

            this.SetTasksAndCancellationToken();
            this.ResetStatistics();
        }

        public void SetTasksAndCancellationToken()
        {
            this._Tasks = new ConcurrentBag<Task>();
            this._TokenSource = new CancellationTokenSource();
            this._Token = this._TokenSource.Token;
        }

        public async void AlgorithmHandler(object parmameter)
        {
            await this.TaskHandler();
            this.CloneUnsortedDatasetIfSorted();
            this.Sorted = false;

            switch (parmameter.ToString())
            {
                case AlgorithmConstants.BubbleSort:
                    await this.AlgorithmRunner(this.BubbleSort);
                    break;
                case AlgorithmConstants.SelectionSort:
                    await this.AlgorithmRunner(this.SelectionSort);  
                    break;
                case AlgorithmConstants.MergeSort:
                    await this.AlgorithmRunner(this.MergeSortDriver);
                    break;
                case AlgorithmConstants.InsertionSort:
                    await this.AlgorithmRunner(this.InsertionSort);
                    break;
                case null:
                    break;
            }

            this.Sorted = true;
        }

        public async void OnWindowClosing(object sender, EventArgs e)
        {
            await this.TaskHandler();
            if (this._TokenSource != null)
            {
                this. _TokenSource.Dispose();
            }
            this.Timer.Close();
        }

        private async Task AlgorithmRunner(Action<CancellationToken> algorithm)
        {
            try
            {
                Task task = new Task(() => algorithm(this._Token), this._Token);
                this._Tasks.Add(task);
                this.Timer.StartTimer();
                task.Start();
                await task;
                this.Timer.EndTimer();
            }
            catch (OperationCanceledException)
            {
            }

            this._TokenSource.Dispose();
        }

        private void BubbleSort(CancellationToken token)
        {
            ChartValues<int> dataset = (ChartValues<int>)this.DataSet[0].Values;
            int count = dataset.Count;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count - i - 1; j++)
                {
                    this.Counter.Count++;
                    if (dataset[j] >= dataset[j + 1])
                    {
                        int holder = dataset[j + 1];
                        dataset[j + 1] = dataset[j];
                        dataset[j] = holder;
                        Thread.Sleep(this.SortingDelay);
                    }

                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                }
            }
        }
        private void SelectionSort(CancellationToken token)
        {
            ChartValues<int> dataset = (ChartValues<int>)this.DataSet[0].Values;
            int count = dataset.Count;
            for (int i = 0; i < count; i++)
            {
                int lowestValue = dataset[i];
                int position = i;
                bool sortOccur = false;

                for (int j = i; j < count; j++)
                {
                    this.Counter.Count++;
                    if (dataset[j] <= lowestValue)
                    {
                        lowestValue = dataset[j];
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
                    int holder = dataset[i];
                    dataset[i] = lowestValue;
                    dataset[position] = holder;
                    Thread.Sleep(this.SortingDelay);
                }
            }
        }

        private void MergeSortDriver(CancellationToken token)
        {
            this.Merge((ChartValues<int>)this.DataSet[0].Values, 0, this.DataSet[0].Values.Count-1, token);
        }

        private void Merge(ChartValues<int> list, int listStartPoint, int listEndPoint, CancellationToken token)
        {
            int midPoint = (listEndPoint - listStartPoint) / 2 + listStartPoint;
            
            if (listStartPoint < midPoint)
            {
                this.Merge(list, listStartPoint, midPoint, token);
            }

            if (midPoint + 1 < listEndPoint)
            {
                this.Merge(list, midPoint + 1, listEndPoint, token);
            }

            this.MergeSort(list, listStartPoint, midPoint, listEndPoint, token);
        }

        private void MergeSort(ChartValues<int> list, int startPoint, int midPoint, int endPoint, CancellationToken token)
        {
            ChartValues<int> tempList = new ChartValues<int>(list.ToArray().DeepClone());
            int leftCounter = startPoint;
            int rightCounter = midPoint + 1;

            for (int i = startPoint; i < endPoint + 1; i++)
            {
                this.Counter.Count++;
                if (token.IsCancellationRequested)
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
                Thread.Sleep(this.SortingDelay);
            }

            return;
        }

        private void InsertionSort(CancellationToken token)
        {
            ChartValues<int> dataset = (ChartValues<int>)this.DataSet[0].Values;
            int count = dataset.Count;

            for (int i = 1; i < count; i++)
            {
                if (dataset[i] < dataset[i - 1])
                {
                    for (int j = 0; j < i; j++)
                    {
                        this.Counter.Count++;
                        if (dataset[i] < dataset[j])
                        {
                            int temp = dataset[i];
                            dataset.RemoveAt(i);
                            dataset.Insert(j, temp);
                            Thread.Sleep(this.SortingDelay);
                            break;
                        }

                        if (token.IsCancellationRequested)
                        {
                            return;
                        }
                    }
                }
            }
        }
    }
}
