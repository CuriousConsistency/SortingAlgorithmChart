using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Algorithm_Chart.Models;
using LiveCharts;
using LiveCharts.Wpf;

namespace Algorithm_Chart.ViewModels
{
    public class AlgorithmViewModel : ICommand
    {
        public SeriesCollection DataSet { get; private set; }

        public int MinimumArraySize { get; } = 10;

        public int MaximumArraySize { get; } = 50;

        private readonly int UpperBound = 100;

        public int ArraySize { get; set; } = 20;

        public ObservableCollection<IAlgorithm> Algorithms { get; private set; }

        public bool Executing { get; private set; }

        public int sortingDelay { get; private set; } = 50;

        private bool Sorted;

        private SeriesCollection UnsortedDataset;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command 
        /// should execute
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                _CanExecuteChanged.Add(value);
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                _CanExecuteChanged.Remove(value);
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Used to hang on to strong references to the event handlers, as
        /// CommandManager.RequerySuggested is static and only holds weak references
        /// (otherwise can get inappropriately garbage collected)
        /// </summary>
        private List<EventHandler> _CanExecuteChanged = new List<EventHandler>();

        public AlgorithmViewModel()
        {
            this.UnsortedDataset = new SeriesCollection
            {
                new ColumnSeries()
            };
            this.DataSet = new SeriesCollection
            {
                new ColumnSeries()
            };
            this.DataSetGenerator();
            this.Executing = false;
            this.Sorted = false;
            this.PopulateAlgorithms();
        }

        public void PopulateAlgorithms()
        {
            this.Algorithms = new ObservableCollection<IAlgorithm>
            {
                new BubbleSort(),
                new SelectionSort()
            };
        }

        public void DataSetGenerator()
        {
            Random rand = new Random();
            this.UnsortedDataset[0].Values = new ChartValues<int>();

            for (int i = 0; i < this.ArraySize; i++)
            {
                this.UnsortedDataset[0].Values.Add(rand.Next(0, this.UpperBound));
            }

            // need to deep clone to this.Dataset
        }

        public bool CanExecute(object parameter)
        {
            return !this.Executing;
        }

        public void Execute(object parameter)
        {
            if (this.CanExecute(null))
            {
                this.Executing = true;
                if (this.Sorted)
                {
                    this.DataSet = this.UnsortedDataset;
                    Thread.Sleep(1000);
                    this.Sorted = false;
                }
                try
                {
                    List<IAlgorithm> algorithmList = this.Algorithms.Where(x => x.Title == (string)parameter).ToList();
                    if (algorithmList != null)
                    {
                        Task.Run(() => algorithmList[0].Algorithm(this.DataSet, this.sortingDelay));
                        this.Sorted = true;
                    }
                }
                finally
                {
                    this.Executing = false;
                }
            }
        }
    }
}
