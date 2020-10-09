using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Algorithm_Chart.Constants;
using Algorithm_Chart.Models;
using Algorithm_Chart.Models.Commands;
using LiveCharts;
using LiveCharts.Wpf;

namespace Algorithm_Chart.ViewModels
{
    public class AlgorithmViewModel : ICommand
    {
        public SeriesCollection DataSet { get; private set; }

        private readonly int startingArraySize = 20;

        private readonly int startingUpperBound = 100;

        public ObservableCollection<IAlgorithm> Algorithms { get; private set; }

        public int sortingDelay { get; private set; } = 50;

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

        public bool Executing { get; private set; }

        public AlgorithmViewModel()
        {
            this.DataSet = this.DataSetGenerator(this.startingArraySize, this.startingUpperBound);
            this.Executing = false;
            this.PopulateAlgorithms();
        }

        public void PopulateAlgorithms()
        {
            this.Algorithms = new ObservableCollection<IAlgorithm>();
            this.Algorithms.Add(new BubbleSort());
        }

        // Need method to create list of buttons, each one uses this class for command, and gets title from each 
        // algorithm dict entry. This name is passed as parameter.

        private SeriesCollection DataSetGenerator(int arraySize, int upperbound)
        {
            Random rand = new Random();
            ChartValues<int> list = new ChartValues<int>();

            for (int i = 0; i < arraySize; i++)
            {
                list.Add(rand.Next(0, upperbound));
            }

            return new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = list
                }
            };
        }

        public bool CanExecute(object parameter)
        {
            return !this.Executing;
        }

        public void Execute(object parameter)
        {
            if (this.CanExecute(null))
            {
                try
                {
                    this.Executing = true;
                    List<IAlgorithm> algorithmList = this.Algorithms.Where(x => x.Title == (string)parameter).ToList();
                    if (algorithmList != null)
                    {
                        Task.Run(() => algorithmList[0].Algorithm(this.DataSet, this.sortingDelay));
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
