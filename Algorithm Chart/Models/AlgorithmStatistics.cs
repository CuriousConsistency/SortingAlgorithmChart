using System.ComponentModel;

namespace Sorting_Algorithm_Chart.Models
{
    public class AlgorithmStatistics : INotifyPropertyChanged
    {
        private string bestCase;
        private string averageCase;
        private string worstCase;
        private string executingAlgorithm;

        public event PropertyChangedEventHandler PropertyChanged;

        public AlgorithmStatistics()
        {
            this.executingAlgorithm = string.Empty;
        }

        public string BestCase
        {
            get
            {
                return this.bestCase;
            }
            set
            {
                this.bestCase = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.BestCase)));
            }
        }

        public string AverageCase
        {
            get
            {
                return this.averageCase;
            }
            set
            {
                this.averageCase = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.AverageCase)));
            }
        }

        public string WorstCase
        {
            get
            {
                return this.worstCase;
            }
            set
            {
                this.worstCase = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.worstCase)));
            }
        }

        public string ExecutingAlgorithm
        {
            get
            {
                return this.executingAlgorithm;
            }
            set
            {
                this.executingAlgorithm = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.executingAlgorithm)));
            }
        }


        public void ResetStatistics()
        {
            this.BestCase = string.Empty;
            this.AverageCase = string.Empty;
            this.WorstCase = string.Empty;
            this.executingAlgorithm = string.Empty;
        }
    }
}
